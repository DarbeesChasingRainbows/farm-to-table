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
    /// Consumer for the low stock event.
    /// </summary>
    public class LowStockEventConsumer : BaseConsumer<InventoryItemLowStockEvent>
    {
        private readonly INotificationService _notificationService;
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="LowStockEventConsumer"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="notificationService">The notification service.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        public LowStockEventConsumer(
            ILogger<LowStockEventConsumer> logger,
            INotificationService notificationService,
            IUnitOfWork unitOfWork
        )
            : base(logger)
        {
            _notificationService = notificationService;
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Consumes a low stock event.
        /// </summary>
        /// <param name="context">The consume context.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        protected override async Task ConsumeMessage(
            ConsumeContext<InventoryItemLowStockEvent> context
        )
        {
            var lowStockEvent = context.Message;

            // Get inventory item details
            var inventoryItem = await _unitOfWork.InventoryItems.GetByIdAsync(lowStockEvent.ItemId);
            if (inventoryItem == null)
            {
                throw new Exception($"Inventory item with ID {lowStockEvent.ItemId} not found");
            }

            // Get location details
            var location = await _unitOfWork.Locations.GetByIdAsync(lowStockEvent.LocationId);
            if (location == null)
            {
                throw new Exception($"Location with ID {lowStockEvent.LocationId} not found");
            }

            // Create notification
            var notification = new Notification
            {
                NotificationType = NotificationType.Email,
                Subject = $"Low Stock Alert: {inventoryItem.Name}",
                Message =
                    $@"<html>
<body>
<h2>Low Stock Alert</h2>
<p>The following item has reached its reorder threshold:</p>
<ul>
<li><strong>Item:</strong> {inventoryItem.Name} ({inventoryItem.SKU})</li>
<li><strong>Location:</strong> {location.Name}</li>
<li><strong>Current Quantity:</strong> {lowStockEvent.CurrentQuantity} {inventoryItem.UnitOfMeasure}</li>
<li><strong>Reorder Threshold:</strong> {lowStockEvent.ReorderThreshold} {inventoryItem.UnitOfMeasure}</li>
</ul>
<p>Please review and reorder as necessary.</p>
</body>
</html>",
                IsHtml = true,
                // For a real application, you would retrieve the appropriate recipients
                Recipients = new List<string>
                {
                    "inventory-manager@example.com",
                    "purchasing@example.com"
                }
            };

            // Send notification
            await _notificationService.SendNotificationAsync(notification);
        }
    }
}
