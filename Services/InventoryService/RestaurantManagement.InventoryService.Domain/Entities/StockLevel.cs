using System;

namespace RestaurantManagement.InventoryService.Domain.Entities
{
    public class StockLevel
    {
        // Required by EF Core
        protected StockLevel() { }

        public StockLevel(string id, string inventoryItemId, string locationId)
        {
            Id = id;
            InventoryItemId = inventoryItemId;
            LocationId = locationId;
            CurrentQuantity = 0;
            ReservedQuantity = 0;
            LastUpdated = DateTime.UtcNow;
        }

        public string Id { get; private set; }
        public string InventoryItemId { get; private set; }
        public string LocationId { get; private set; }
        public double CurrentQuantity { get; private set; }
        public double ReservedQuantity { get; private set; }
        public DateTime LastUpdated { get; private set; }

        // Navigation properties
        public InventoryItem InventoryItem { get; private set; }

        // Computed property
        public double AvailableQuantity => CurrentQuantity - ReservedQuantity;

        public void UpdateQuantity(double newQuantity)
        {
            if (newQuantity < 0)
                newQuantity = 0;

            CurrentQuantity = newQuantity;
            LastUpdated = DateTime.UtcNow;
        }

        public void IncreaseQuantity(double quantityToAdd)
        {
            if (quantityToAdd <= 0)
                return;

            CurrentQuantity += quantityToAdd;
            LastUpdated = DateTime.UtcNow;
        }

        public void DecreaseQuantity(double quantityToRemove)
        {
            if (quantityToRemove <= 0)
                return;

            if (CurrentQuantity - quantityToRemove < 0)
                CurrentQuantity = 0;
            else
                CurrentQuantity -= quantityToRemove;

            LastUpdated = DateTime.UtcNow;
        }

        public void Reserve(double quantity)
        {
            if (quantity <= 0)
                return;

            if (AvailableQuantity < quantity)
                return;

            ReservedQuantity += quantity;
            LastUpdated = DateTime.UtcNow;
        }

        public void Release(double quantity)
        {
            if (quantity <= 0)
                return;

            if (ReservedQuantity < quantity)
                ReservedQuantity = 0;
            else
                ReservedQuantity -= quantity;

            LastUpdated = DateTime.UtcNow;
        }

        public bool IsAvailable(double requiredQuantity)
        {
            return AvailableQuantity >= requiredQuantity;
        }
    }
}
