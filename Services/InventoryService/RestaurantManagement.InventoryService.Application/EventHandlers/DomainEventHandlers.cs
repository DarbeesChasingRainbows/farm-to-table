using MediatR;
using Microsoft.Extensions.Logging;
using RestaurantManagement.InventoryService.Application.Interfaces;
using RestaurantManagement.InventoryService.Application.Models;
using RestaurantManagement.InventoryService.Domain.Entities;
using RestaurantManagement.InventoryService.Domain.Events;
using RestaurantManagement.InventoryService.Domain.Repositories;

namespace RestaurantManagement.InventoryService.Application.EventHandlers
{
    /// <summary>
    /// Handler for the StockLevelBelowThresholdEvent
    /// </summary>
    public class StockLevelBelowThresholdEventHandler
        : INotificationHandler<StockLevelBelowThresholdEvent>
    {
        private readonly IInventoryItemRepository _inventoryItemRepository;
        private readonly ILocationRepository _locationRepository;
        private readonly INotificationService _notificationService;
        private readonly ILogger<StockLevelBelowThresholdEventHandler> _logger;

        public StockLevelBelowThresholdEventHandler(
            IInventoryItemRepository inventoryItemRepository,
            ILocationRepository locationRepository,
            INotificationService notificationService,
            ILogger<StockLevelBelowThresholdEventHandler> logger
        )
        {
            _inventoryItemRepository = inventoryItemRepository;
            _locationRepository = locationRepository;
            _notificationService = notificationService;
            _logger = logger;
        }

        public async Task Handle(
            StockLevelBelowThresholdEvent notification,
            CancellationToken cancellationToken
        )
        {
            _logger.LogInformation(
                "Handling StockLevelBelowThresholdEvent for inventory item {InventoryItemId} at location {LocationId}",
                notification.InventoryItemId,
                notification.LocationId
            );

            // Get the inventory item details
            var inventoryItem = await _inventoryItemRepository.GetByIdAsync(
                notification.InventoryItemId,
                cancellationToken
            );
            if (inventoryItem == null)
            {
                _logger.LogWarning(
                    "Inventory item {InventoryItemId} not found",
                    notification.InventoryItemId
                );
                return;
            }

            // Get the location details
            var location = await _locationRepository.GetByIdAsync(
                notification.LocationId,
                cancellationToken
            );
            if (location == null)
            {
                _logger.LogWarning("Location {LocationId} not found", notification.LocationId);
                return;
            }

            // Create the notification message
            var notificationMessage = new Notification
            {
                NotificationType = NotificationType.Email,
                Subject = $"Low Stock Alert: {inventoryItem.Name} at {location.Name}",
                Message =
                    $"The stock level for {inventoryItem.Name} (SKU: {inventoryItem.SKU}) at {location.Name} has fallen below the reorder threshold. "
                    + $"Current quantity: {notification.CurrentQuantity} {inventoryItem.UnitOfMeasure}. "
                    + $"Reorder threshold: {inventoryItem.ReorderThreshold} {inventoryItem.UnitOfMeasure}. "
                    + $"Recommended order quantity: {inventoryItem.DefaultOrderQuantity} {inventoryItem.UnitOfMeasure}.",
                Recipients = new List<string> { "inventory-manager@restaurant.example.com" }, // This would be configured or pulled from a repository
                IsHtml = false,
                Data = new Dictionary<string, string>
                {
                    { "InventoryItemId", inventoryItem.Id },
                    { "LocationId", location.Id },
                    { "CurrentQuantity", notification.CurrentQuantity.ToString() },
                    { "ReorderThreshold", inventoryItem.ReorderThreshold.ToString() },
                    { "DefaultOrderQuantity", inventoryItem.DefaultOrderQuantity.ToString() }
                }
            };

            // Send the notification
            await _notificationService.SendNotificationAsync(notificationMessage);

            _logger.LogInformation(
                "Sent low stock notification for inventory item {InventoryItemId} at location {LocationId}",
                notification.InventoryItemId,
                notification.LocationId
            );
        }
    }

    /// <summary>
    /// Handler for the BatchExpiringEvent
    /// </summary>
    public class BatchExpiringEventHandler : INotificationHandler<BatchExpiringEvent>
    {
        private readonly IInventoryItemRepository _inventoryItemRepository;
        private readonly ILocationRepository _locationRepository;
        private readonly IBatchRepository _batchRepository;
        private readonly INotificationService _notificationService;
        private readonly ILogger<BatchExpiringEventHandler> _logger;

        public BatchExpiringEventHandler(
            IInventoryItemRepository inventoryItemRepository,
            ILocationRepository locationRepository,
            IBatchRepository batchRepository,
            INotificationService notificationService,
            ILogger<BatchExpiringEventHandler> logger
        )
        {
            _inventoryItemRepository = inventoryItemRepository;
            _locationRepository = locationRepository;
            _batchRepository = batchRepository;
            _notificationService = notificationService;
            _logger = logger;
        }

        public async Task Handle(
            BatchExpiringEvent notification,
            CancellationToken cancellationToken
        )
        {
            _logger.LogInformation(
                "Handling BatchExpiringEvent for batch {BatchId}",
                notification.BatchId
            );

            // Get the batch details
            var batch = await _batchRepository.GetByIdAsync(
                notification.BatchId,
                cancellationToken
            );
            if (batch == null)
            {
                _logger.LogWarning("Batch {BatchId} not found", notification.BatchId);
                return;
            }

            // Get the inventory item details
            var inventoryItem = await _inventoryItemRepository.GetByIdAsync(
                batch.InventoryItemId,
                cancellationToken
            );
            if (inventoryItem == null)
            {
                _logger.LogWarning(
                    "Inventory item {InventoryItemId} not found",
                    batch.InventoryItemId
                );
                return;
            }

            // Get the location details
            var location = await _locationRepository.GetByIdAsync(
                batch.LocationId,
                cancellationToken
            );
            if (location == null)
            {
                _logger.LogWarning("Location {LocationId} not found", batch.LocationId);
                return;
            }

            // Calculate days until expiration
            var daysUntilExpiration = (batch.ExpirationDate - DateTime.UtcNow).Days;
            var expirationText =
                daysUntilExpiration > 0
                    ? $"will expire in {daysUntilExpiration} days"
                    : "has expired";

            // Create the notification message
            var notificationMessage = new Notification
            {
                NotificationType = NotificationType.Email,
                Subject = $"Batch Expiration Alert: {inventoryItem.Name} at {location.Name}",
                Message =
                    $"A batch of {inventoryItem.Name} (SKU: {inventoryItem.SKU}) at {location.Name} {expirationText}. "
                    + $"Batch #: {batch.BatchNumber}. "
                    + $"Expiration date: {batch.ExpirationDate:yyyy-MM-dd}. "
                    + $"Quantity: {batch.RemainingQuantity} {inventoryItem.UnitOfMeasure}. "
                    + $"Please take appropriate action to minimize waste.",
                Recipients = new List<string>
                {
                    "inventory-manager@restaurant.example.com",
                    "chef@restaurant.example.com"
                }, // This would be configured or pulled from a repository
                IsHtml = false,
                Data = new Dictionary<string, string>
                {
                    { "BatchId", batch.Id },
                    { "InventoryItemId", inventoryItem.Id },
                    { "LocationId", location.Id },
                    { "BatchNumber", batch.BatchNumber },
                    { "ExpirationDate", batch.ExpirationDate.ToString("o") },
                    { "RemainingQuantity", batch.RemainingQuantity.ToString() }
                }
            };

            // Send the notification
            await _notificationService.SendNotificationAsync(notificationMessage);

            _logger.LogInformation(
                "Sent batch expiration notification for batch {BatchId}",
                notification.BatchId
            );
        }
    }

    /// <summary>
    /// Handler for the InventoryTransactionCompletedEvent
    /// </summary>
    public class InventoryTransactionCompletedEventHandler
        : INotificationHandler<InventoryTransactionCompletedEvent>
    {
        private readonly IInventoryItemRepository _inventoryItemRepository;
        private readonly IStockLevelRepository _stockLevelRepository;
        private readonly ILocationRepository _locationRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<InventoryTransactionCompletedEventHandler> _logger;

        public InventoryTransactionCompletedEventHandler(
            IInventoryItemRepository inventoryItemRepository,
            IStockLevelRepository stockLevelRepository,
            ILocationRepository locationRepository,
            IUnitOfWork unitOfWork,
            ILogger<InventoryTransactionCompletedEventHandler> logger
        )
        {
            _inventoryItemRepository = inventoryItemRepository;
            _stockLevelRepository = stockLevelRepository;
            _locationRepository = locationRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task Handle(
            InventoryTransactionCompletedEvent notification,
            CancellationToken cancellationToken
        )
        {
            _logger.LogInformation(
                "Handling InventoryTransactionCompletedEvent for transaction {TransactionId}",
                notification.TransactionId
            );

            // Handle different transaction types
            switch (notification.TransactionType)
            {
                case "Receipt":
                    await HandleReceiptTransaction(notification, cancellationToken);
                    break;

                case "Consumption":
                    await HandleConsumptionTransaction(notification, cancellationToken);
                    break;

                case "Transfer":
                    await HandleTransferTransaction(notification, cancellationToken);
                    break;

                case "Adjustment":
                    await HandleAdjustmentTransaction(notification, cancellationToken);
                    break;

                default:
                    _logger.LogWarning(
                        "Unknown transaction type {TransactionType} for transaction {TransactionId}",
                        notification.TransactionType,
                        notification.TransactionId
                    );
                    break;
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        private async Task HandleReceiptTransaction(
            InventoryTransactionCompletedEvent notification,
            CancellationToken cancellationToken
        )
        {
            _logger.LogInformation(
                "Handling receipt transaction {TransactionId}",
                notification.TransactionId
            );

            // Get the stock level for the destination location
            var stockLevel = await _stockLevelRepository.GetByItemAndLocationAsync(
                notification.InventoryItemId,
                notification.DestinationLocationId!,
                cancellationToken
            );

            if (stockLevel == null)
            {
                // Create a new stock level if it doesn't exist
                _logger.LogInformation(
                    "Creating new stock level for item {InventoryItemId} at location {LocationId}",
                    notification.InventoryItemId,
                    notification.DestinationLocationId
                );

                stockLevel = new StockLevel
                {
                    Id = Guid.NewGuid().ToString(),
                    InventoryItemId = notification.InventoryItemId,
                    LocationId = notification.DestinationLocationId!,
                    CurrentQuantity = notification.Quantity,
                    ReservedQuantity = 0,
                    AvailableQuantity = notification.Quantity,
                    LastUpdatedBy = notification.UserId,
                    LastUpdatedAt = DateTime.UtcNow
                };

                await _stockLevelRepository.AddAsync(stockLevel, cancellationToken);
            }
            else
            {
                // Update existing stock level
                _logger.LogInformation(
                    "Updating stock level {StockLevelId} for item {InventoryItemId} at location {LocationId}",
                    stockLevel.Id,
                    notification.InventoryItemId,
                    notification.DestinationLocationId
                );

                stockLevel.CurrentQuantity += notification.Quantity;
                stockLevel.AvailableQuantity += notification.Quantity;
                stockLevel.LastUpdatedBy = notification.UserId;
                stockLevel.LastUpdatedAt = DateTime.UtcNow;

                _stockLevelRepository.Update(stockLevel);
            }
        }

        private async Task HandleConsumptionTransaction(
            InventoryTransactionCompletedEvent notification,
            CancellationToken cancellationToken
        )
        {
            _logger.LogInformation(
                "Handling consumption transaction {TransactionId}",
                notification.TransactionId
            );

            // Get the stock level for the source location
            var stockLevel = await _stockLevelRepository.GetByItemAndLocationAsync(
                notification.InventoryItemId,
                notification.SourceLocationId!,
                cancellationToken
            );

            if (stockLevel == null)
            {
                _logger.LogWarning(
                    "Stock level not found for item {InventoryItemId} at location {LocationId}",
                    notification.InventoryItemId,
                    notification.SourceLocationId
                );
                return;
            }

            // Verify sufficient quantity
            if (stockLevel.CurrentQuantity < notification.Quantity)
            {
                _logger.LogWarning(
                    "Insufficient quantity for consumption transaction {TransactionId}. Available: {Available}, Required: {Required}",
                    notification.TransactionId,
                    stockLevel.CurrentQuantity,
                    notification.Quantity
                );
                return;
            }

            // Update stock level
            stockLevel.CurrentQuantity -= notification.Quantity;
            stockLevel.AvailableQuantity = Math.Max(
                0,
                stockLevel.CurrentQuantity - stockLevel.ReservedQuantity
            );
            stockLevel.LastUpdatedBy = notification.UserId;
            stockLevel.LastUpdatedAt = DateTime.UtcNow;

            _stockLevelRepository.Update(stockLevel);
        }

        private async Task HandleTransferTransaction(
            InventoryTransactionCompletedEvent notification,
            CancellationToken cancellationToken
        )
        {
            _logger.LogInformation(
                "Handling transfer transaction {TransactionId}",
                notification.TransactionId
            );

            // Validate source and destination locations
            if (
                string.IsNullOrEmpty(notification.SourceLocationId)
                || string.IsNullOrEmpty(notification.DestinationLocationId)
            )
            {
                _logger.LogWarning(
                    "Invalid source or destination location for transfer transaction {TransactionId}",
                    notification.TransactionId
                );
                return;
            }

            // Get the source stock level
            var sourceStockLevel = await _stockLevelRepository.GetByItemAndLocationAsync(
                notification.InventoryItemId,
                notification.SourceLocationId,
                cancellationToken
            );

            if (sourceStockLevel == null)
            {
                _logger.LogWarning(
                    "Source stock level not found for item {InventoryItemId} at location {LocationId}",
                    notification.InventoryItemId,
                    notification.SourceLocationId
                );
                return;
            }

            // Verify sufficient quantity
            if (sourceStockLevel.CurrentQuantity < notification.Quantity)
            {
                _logger.LogWarning(
                    "Insufficient quantity for transfer transaction {TransactionId}. Available: {Available}, Required: {Required}",
                    notification.TransactionId,
                    sourceStockLevel.CurrentQuantity,
                    notification.Quantity
                );
                return;
            }

            // Get or create destination stock level
            var destinationStockLevel = await _stockLevelRepository.GetByItemAndLocationAsync(
                notification.InventoryItemId,
                notification.DestinationLocationId,
                cancellationToken
            );

            if (destinationStockLevel == null)
            {
                // Create a new stock level if it doesn't exist
                _logger.LogInformation(
                    "Creating new destination stock level for item {InventoryItemId} at location {LocationId}",
                    notification.InventoryItemId,
                    notification.DestinationLocationId
                );

                destinationStockLevel = new StockLevel
                {
                    Id = Guid.NewGuid().ToString(),
                    InventoryItemId = notification.InventoryItemId,
                    LocationId = notification.DestinationLocationId,
                    CurrentQuantity = notification.Quantity,
                    ReservedQuantity = 0,
                    AvailableQuantity = notification.Quantity,
                    LastUpdatedBy = notification.UserId,
                    LastUpdatedAt = DateTime.UtcNow
                };

                await _stockLevelRepository.AddAsync(destinationStockLevel, cancellationToken);
            }
            else
            {
                // Update existing destination stock level
                _logger.LogInformation(
                    "Updating destination stock level {StockLevelId} for item {InventoryItemId} at location {LocationId}",
                    destinationStockLevel.Id,
                    notification.InventoryItemId,
                    notification.DestinationLocationId
                );

                destinationStockLevel.CurrentQuantity += notification.Quantity;
                destinationStockLevel.AvailableQuantity += notification.Quantity;
                destinationStockLevel.LastUpdatedBy = notification.UserId;
                destinationStockLevel.LastUpdatedAt = DateTime.UtcNow;

                _stockLevelRepository.Update(destinationStockLevel);
            }

            // Update source stock level
            sourceStockLevel.CurrentQuantity -= notification.Quantity;
            sourceStockLevel.AvailableQuantity = Math.Max(
                0,
                sourceStockLevel.CurrentQuantity - sourceStockLevel.ReservedQuantity
            );
            sourceStockLevel.LastUpdatedBy = notification.UserId;
            sourceStockLevel.LastUpdatedAt = DateTime.UtcNow;

            _stockLevelRepository.Update(sourceStockLevel);
        }

        private async Task HandleAdjustmentTransaction(
            InventoryTransactionCompletedEvent notification,
            CancellationToken cancellationToken
        )
        {
            _logger.LogInformation(
                "Handling adjustment transaction {TransactionId}",
                notification.TransactionId
            );

            // Get the stock level for the location
            var stockLevel = await _stockLevelRepository.GetByItemAndLocationAsync(
                notification.InventoryItemId,
                notification.DestinationLocationId!,
                cancellationToken
            );

            if (stockLevel == null)
            {
                // Create a new stock level if it doesn't exist
                _logger.LogInformation(
                    "Creating new stock level for item {InventoryItemId} at location {LocationId}",
                    notification.InventoryItemId,
                    notification.DestinationLocationId
                );

                stockLevel = new StockLevel
                {
                    Id = Guid.NewGuid().ToString(),
                    InventoryItemId = notification.InventoryItemId,
                    LocationId = notification.DestinationLocationId!,
                    CurrentQuantity = notification.Quantity, // Absolute quantity for adjustment
                    ReservedQuantity = 0,
                    AvailableQuantity = notification.Quantity,
                    LastUpdatedBy = notification.UserId,
                    LastUpdatedAt = DateTime.UtcNow
                };

                await _stockLevelRepository.AddAsync(stockLevel, cancellationToken);
            }
            else
            {
                // Update existing stock level with the new quantity
                _logger.LogInformation(
                    "Updating stock level {StockLevelId} for item {InventoryItemId} at location {LocationId}",
                    stockLevel.Id,
                    notification.InventoryItemId,
                    notification.DestinationLocationId
                );

                // For adjustments, Quantity represents the new total quantity
                stockLevel.CurrentQuantity = notification.Quantity;
                stockLevel.AvailableQuantity = Math.Max(
                    0,
                    notification.Quantity - stockLevel.ReservedQuantity
                );
                stockLevel.LastUpdatedBy = notification.UserId;
                stockLevel.LastUpdatedAt = DateTime.UtcNow;

                _stockLevelRepository.Update(stockLevel);
            }
        }
    }
}
