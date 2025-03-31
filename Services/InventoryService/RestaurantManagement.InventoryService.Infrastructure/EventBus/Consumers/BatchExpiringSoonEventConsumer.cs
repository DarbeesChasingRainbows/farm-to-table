using MassTransit;
using Microsoft.Extensions.Logging;
using RestaurantManagement.InventoryService.Application.Interfaces;
using RestaurantManagement.InventoryService.Application.Models;
using RestaurantManagement.InventoryService.Domain.Events;
using RestaurantManagement.InventoryService.Infrastructure.Data.Repositories.Interfaces;
using RestaurantManagement.InventoryService.Infrastructure.Data.UnitOfWork;

namespace RestaurantManagement.InventoryService.Infrastructure.EventBus.Consumers
{
    /// <summary>
    /// Consumer for the batch expiring soon event.
    /// </summary>
    public class BatchExpiringSoonEventConsumer : BaseConsumer<BatchExpiringSoonEvent>
    {
        private readonly INotificationService _notificationService;
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="BatchExpiringSoonEventConsumer"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="notificationService">The notification service.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        public BatchExpiringSoonEventConsumer(
            ILogger<BatchExpiringSoonEventConsumer> logger,
            INotificationService notificationService,
            IUnitOfWork unitOfWork
        )
            : base(logger)
        {
            _notificationService = notificationService;
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Consumes a batch expiring soon event.
        /// </summary>
        /// <param name="context">The consume context.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        protected override async Task ConsumeMessage(ConsumeContext<BatchExpiringSoonEvent> context)
        {
            var batchExpiringEvent = context.Message;

            // Get inventory item details
            var inventoryItem = await _unitOfWork.InventoryItems.GetByIdAsync(
                batchExpiringEvent.InventoryItemId
            );
            if (inventoryItem == null)
            {
                throw new Exception(
                    $"Inventory item with ID {batchExpiringEvent.InventoryItemId} not found"
                );
            }

            // Get batch details
            var batch = await _unitOfWork.Batches.GetByIdAsync(batchExpiringEvent.BatchId);
            if (batch == null)
            {
                throw new Exception($"Batch with ID {batchExpiringEvent.BatchId} not found");
            }

            // Get location details
            var location = await _unitOfWork.Locations.GetByIdAsync(batch.LocationId);
            if (location == null)
            {
                throw new Exception($"Location with ID {batch.LocationId} not found");
            }

            // Create notification
            var notification = new Notification
            {
                NotificationType = NotificationType.Email,
                Subject = $"Expiring Item Alert: {inventoryItem.Name} (Batch {batch.BatchNumber})",
                Message =
                    $@"<html>
<body>
<h2>Expiring Item Alert</h2>
<p>The following item batch is expiring soon:</p>
<ul>
<li><strong>Item:</strong> {inventoryItem.Name} ({inventoryItem.SKU})</li>
<li><strong>Batch Number:</strong> {batch.BatchNumber}</li>
<li><strong>Location:</strong> {location.Name}</li>
<li><strong>Remaining Quantity:</strong> {batchExpiringEvent.RemainingQuantity} {inventoryItem.UnitOfMeasure}</li>
<li><strong>Expiration Date:</strong> {batchExpiringEvent.ExpirationDate:yyyy-MM-dd}</li>
<li><strong>Days Until Expiration:</strong> {batchExpiringEvent.DaysUntilExpiration}</li>
</ul>
<p>Please review and take appropriate action to prevent waste.</p>
</body>
</html>",
                IsHtml = true,
                // For a real application, you would retrieve the appropriate recipients
                Recipients = new List<string>
                {
                    "inventory-manager@example.com",
                    "kitchen-manager@example.com"
                }
            };

            // Send notification
            await _notificationService.SendNotificationAsync(notification);
        }
    }
}
