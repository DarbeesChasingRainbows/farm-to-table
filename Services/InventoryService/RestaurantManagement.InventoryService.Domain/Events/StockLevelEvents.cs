using System;

namespace RestaurantManagement.InventoryService.Domain.Events
{
    public abstract class StockLevelEvent : BaseDomainEvent
    {
        public string StockLevelId { get; }
        public string InventoryItemId { get; }
        public string LocationId { get; }

        protected StockLevelEvent(
            string stockLevelId,
            string inventoryItemId,
            string locationId,
            string userId
        )
            : base(userId)
        {
            StockLevelId = stockLevelId ?? throw new ArgumentNullException(nameof(stockLevelId));
            InventoryItemId =
                inventoryItemId ?? throw new ArgumentNullException(nameof(inventoryItemId));
            LocationId = locationId ?? throw new ArgumentNullException(nameof(locationId));
        }
    }

    public class StockLevelCreatedEvent : StockLevelEvent
    {
        public StockLevelCreatedEvent(
            string stockLevelId,
            string inventoryItemId,
            string locationId,
            string userId
        )
            : base(stockLevelId, inventoryItemId, locationId, userId) { }
    }

    public class StockLevelQuantityUpdatedEvent : StockLevelEvent
    {
        public double OldQuantity { get; }
        public double NewQuantity { get; }

        public StockLevelQuantityUpdatedEvent(
            string stockLevelId,
            string inventoryItemId,
            string locationId,
            double oldQuantity,
            double newQuantity,
            string userId
        )
            : base(stockLevelId, inventoryItemId, locationId, userId)
        {
            OldQuantity = oldQuantity;
            NewQuantity = newQuantity;
        }
    }

    public class StockLevelQuantityIncreasedEvent : StockLevelEvent
    {
        public double PreviousQuantity { get; }
        public double QuantityAdded { get; }
        public double NewQuantity { get; }

        public StockLevelQuantityIncreasedEvent(
            string stockLevelId,
            string inventoryItemId,
            string locationId,
            double previousQuantity,
            double quantityAdded,
            double newQuantity,
            string userId
        )
            : base(stockLevelId, inventoryItemId, locationId, userId)
        {
            PreviousQuantity = previousQuantity;
            QuantityAdded = quantityAdded;
            NewQuantity = newQuantity;
        }
    }

    public class StockLevelQuantityDecreasedEvent : StockLevelEvent
    {
        public double PreviousQuantity { get; }
        public double QuantityRemoved { get; }
        public double NewQuantity { get; }

        public StockLevelQuantityDecreasedEvent(
            string stockLevelId,
            string inventoryItemId,
            string locationId,
            double previousQuantity,
            double quantityRemoved,
            double newQuantity,
            string userId
        )
            : base(stockLevelId, inventoryItemId, locationId, userId)
        {
            PreviousQuantity = previousQuantity;
            QuantityRemoved = quantityRemoved;
            NewQuantity = newQuantity;
        }
    }

    public class StockReservedEvent : StockLevelEvent
    {
        public double QuantityReserved { get; }
        public double TotalReservedQuantity { get; }
        public double AvailableQuantity { get; }

        public StockReservedEvent(
            string stockLevelId,
            string inventoryItemId,
            string locationId,
            double quantityReserved,
            double totalReservedQuantity,
            double availableQuantity,
            string userId
        )
            : base(stockLevelId, inventoryItemId, locationId, userId)
        {
            QuantityReserved = quantityReserved;
            TotalReservedQuantity = totalReservedQuantity;
            AvailableQuantity = availableQuantity;
        }
    }

    public class StockReleasedEvent : StockLevelEvent
    {
        public double QuantityReleased { get; }
        public double TotalReservedQuantity { get; }
        public double AvailableQuantity { get; }

        public StockReleasedEvent(
            string stockLevelId,
            string inventoryItemId,
            string locationId,
            double quantityReleased,
            double totalReservedQuantity,
            double availableQuantity,
            string userId
        )
            : base(stockLevelId, inventoryItemId, locationId, userId)
        {
            QuantityReleased = quantityReleased;
            TotalReservedQuantity = totalReservedQuantity;
            AvailableQuantity = availableQuantity;
        }
    }

    public class InsufficientStockAvailableEvent : StockLevelEvent
    {
        public double RequestedQuantity { get; }
        public double AvailableQuantity { get; }

        public InsufficientStockAvailableEvent(
            string stockLevelId,
            string inventoryItemId,
            string locationId,
            double requestedQuantity,
            double availableQuantity,
            string userId
        )
            : base(stockLevelId, inventoryItemId, locationId, userId)
        {
            RequestedQuantity = requestedQuantity;
            AvailableQuantity = availableQuantity;
        }
    }
}
