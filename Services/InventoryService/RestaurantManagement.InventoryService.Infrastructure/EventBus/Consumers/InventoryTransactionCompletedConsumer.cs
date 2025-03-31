using MassTransit;
using Microsoft.Extensions.Logging;
using RestaurantManagement.InventoryService.Domain.Events;
using RestaurantManagement.InventoryService.Infrastructure.Data.UnitOfWork;

namespace RestaurantManagement.InventoryService.Infrastructure.EventBus.Consumers
{
    /// <summary>
    /// Consumer for the inventory transaction completed event.
    /// </summary>
    public class InventoryTransactionCompletedConsumer
        : BaseConsumer<InventoryTransactionCompletedEvent>
    {
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="InventoryTransactionCompletedConsumer"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        public InventoryTransactionCompletedConsumer(
            ILogger<InventoryTransactionCompletedConsumer> logger,
            IUnitOfWork unitOfWork
        )
            : base(logger)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Consumes an inventory transaction completed event.
        /// </summary>
        /// <param name="context">The consume context.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        protected override async Task ConsumeMessage(
            ConsumeContext<InventoryTransactionCompletedEvent> context
        )
        {
            var transactionCompletedEvent = context.Message;

            // Get the transaction details
            var transaction = await _unitOfWork.InventoryTransactions.GetWithItemsAsync(
                transactionCompletedEvent.TransactionId
            );
            if (transaction == null)
            {
                throw new Exception(
                    $"Inventory transaction with ID {transactionCompletedEvent.TransactionId} not found"
                );
            }

            // Handle transaction based on type
            switch (transaction.Type.ToLower())
            {
                case "received":
                    await HandleReceivedTransaction(transaction, transactionCompletedEvent);
                    break;

                case "consumed":
                    await HandleConsumedTransaction(transaction, transactionCompletedEvent);
                    break;

                case "transferred":
                    await HandleTransferredTransaction(transaction, transactionCompletedEvent);
                    break;

                case "returned":
                    await HandleReturnedTransaction(transaction, transactionCompletedEvent);
                    break;

                case "adjustment":
                    await HandleAdjustmentTransaction(transaction, transactionCompletedEvent);
                    break;

                default:
                    // Unknown transaction type - log warning
                    break;
            }

            // Save all changes
            await _unitOfWork.SaveChangesAsync();
        }

        private async Task HandleReceivedTransaction(
            InventoryTransaction transaction,
            InventoryTransactionCompletedEvent transactionEvent
        )
        {
            // For received items, increase stock levels and create batches if applicable
            foreach (var item in transaction.Items)
            {
                // Increase stock level
                await _unitOfWork.StockLevels.UpdateStockQuantityAsync(
                    item.InventoryItemId,
                    item.LocationId,
                    (
                        await _unitOfWork.StockLevels.GetAvailableQuantityForItemAsync(
                            item.InventoryItemId,
                            item.LocationId
                        )
                    ) + item.Quantity
                );

                // Update inventory item cost if provided
                if (item.UnitCost.HasValue)
                {
                    var inventoryItem = await _unitOfWork.InventoryItems.GetByIdAsync(
                        item.InventoryItemId
                    );
                    if (inventoryItem != null)
                    {
                        inventoryItem.UpdateCost(item.UnitCost.Value, transaction.UserId);
                    }
                }

                // Check for low stock after receiving and potentially cancel any pending alerts
                // This would be implemented in a real application
            }
        }

        private async Task HandleConsumedTransaction(
            InventoryTransaction transaction,
            InventoryTransactionCompletedEvent transactionEvent
        )
        {
            // For consumed items, decrease stock levels and consume from batches if applicable
            foreach (var item in transaction.Items)
            {
                // Decrease stock level
                await _unitOfWork.StockLevels.UpdateStockQuantityAsync(
                    item.InventoryItemId,
                    item.LocationId,
                    (
                        await _unitOfWork.StockLevels.GetAvailableQuantityForItemAsync(
                            item.InventoryItemId,
                            item.LocationId
                        )
                    ) - item.Quantity
                );

                // Consume from batch if specified
                if (!string.IsNullOrEmpty(item.BatchId))
                {
                    var batch = await _unitOfWork.Batches.GetByIdAsync(item.BatchId);
                    if (batch != null)
                    {
                        batch.Consume(item.Quantity);
                    }
                }

                // Check for low stock after consumption
                var stockLevel = await _unitOfWork.StockLevels.GetByItemAndLocationAsync(
                    item.InventoryItemId,
                    item.LocationId
                );
                var inventoryItem = await _unitOfWork.InventoryItems.GetByIdAsync(
                    item.InventoryItemId
                );

                if (
                    stockLevel != null
                    && inventoryItem != null
                    && stockLevel.CurrentQuantity <= inventoryItem.ReorderThreshold
                )
                {
                    // This would trigger the LowStockEvent - in a real implementation, this would be published
                    // For now, we'll just log that it would be published
                }
            }
        }

        private async Task HandleTransferredTransaction(
            InventoryTransaction transaction,
            InventoryTransactionCompletedEvent transactionEvent
        )
        {
            // For transferred items, decrease stock at source location and increase at destination
            if (string.IsNullOrEmpty(transaction.DestinationLocationId))
            {
                throw new Exception("Destination location is required for transfer transactions");
            }

            foreach (var item in transaction.Items)
            {
                // Decrease stock at source location
                await _unitOfWork.StockLevels.UpdateStockQuantityAsync(
                    item.InventoryItemId,
                    item.LocationId,
                    (
                        await _unitOfWork.StockLevels.GetAvailableQuantityForItemAsync(
                            item.InventoryItemId,
                            item.LocationId
                        )
                    ) - item.Quantity
                );

                // Increase stock at destination location
                await _unitOfWork.StockLevels.UpdateStockQuantityAsync(
                    item.InventoryItemId,
                    transaction.DestinationLocationId,
                    (
                        await _unitOfWork.StockLevels.GetAvailableQuantityForItemAsync(
                            item.InventoryItemId,
                            transaction.DestinationLocationId
                        )
                    ) + item.Quantity
                );

                // Transfer batch if specified
                if (!string.IsNullOrEmpty(item.BatchId))
                {
                    var batch = await _unitOfWork.Batches.GetByIdAsync(item.BatchId);
                    if (batch != null)
                    {
                        batch.Transfer(transaction.DestinationLocationId);
                    }
                }

                // Check for low stock after transfer at both locations
                // This would be implemented in a real application
            }
        }

        private async Task HandleReturnedTransaction(
            InventoryTransaction transaction,
            InventoryTransactionCompletedEvent transactionEvent
        )
        {
            // For returned items, decrease stock levels
            foreach (var item in transaction.Items)
            {
                await _unitOfWork.StockLevels.UpdateStockQuantityAsync(
                    item.InventoryItemId,
                    item.LocationId,
                    (
                        await _unitOfWork.StockLevels.GetAvailableQuantityForItemAsync(
                            item.InventoryItemId,
                            item.LocationId
                        )
                    ) - item.Quantity
                );

                // If returning to vendor, potentially update vendor metrics
                // This would be implemented in a real application
            }
        }

        private async Task HandleAdjustmentTransaction(
            InventoryTransaction transaction,
            InventoryTransactionCompletedEvent transactionEvent
        )
        {
            // For adjustments, directly set the stock levels to the specified quantities
            foreach (var item in transaction.Items)
            {
                await _unitOfWork.StockLevels.UpdateStockQuantityAsync(
                    item.InventoryItemId,
                    item.LocationId,
                    item.Quantity
                );

                // Check for low stock after adjustment
                // This would be implemented in a real application
            }
        }
    }
}
