using System;

namespace RestaurantManagement.InventoryService.Domain.Entities
{
    public class Batch
    {
        // Required by EF Core
        protected Batch() { }

        public Batch(
            string id,
            string inventoryItemId,
            string batchNumber,
            DateTime receivedDate,
            DateTime expirationDate,
            double initialQuantity,
            decimal unitCost,
            string locationId
        )
        {
            Id = id;
            InventoryItemId = inventoryItemId;
            BatchNumber = batchNumber;
            ReceivedDate = receivedDate;
            ExpirationDate = expirationDate;
            InitialQuantity = initialQuantity;
            RemainingQuantity = initialQuantity;
            UnitCost = unitCost;
            LocationId = locationId;
        }

        public string Id { get; private set; }
        public string InventoryItemId { get; private set; }
        public string BatchNumber { get; private set; }
        public DateTime ReceivedDate { get; private set; }
        public DateTime ExpirationDate { get; private set; }
        public double InitialQuantity { get; private set; }
        public double RemainingQuantity { get; private set; }
        public decimal UnitCost { get; private set; }
        public string LocationId { get; private set; }
        public string VendorId { get; private set; }
        public string PurchaseOrderId { get; private set; }

        // Navigation properties
        public InventoryItem InventoryItem { get; private set; }

        public void SetVendorInfo(string vendorId, string purchaseOrderId)
        {
            VendorId = vendorId;
            PurchaseOrderId = purchaseOrderId;
        }

        public void UpdateRemainingQuantity(double newRemainingQuantity)
        {
            if (newRemainingQuantity < 0)
                newRemainingQuantity = 0;

            if (newRemainingQuantity > InitialQuantity)
                newRemainingQuantity = InitialQuantity;

            RemainingQuantity = newRemainingQuantity;
        }

        public void Consume(double quantity)
        {
            if (quantity <= 0)
                return;

            if (quantity > RemainingQuantity)
                RemainingQuantity = 0;
            else
                RemainingQuantity -= quantity;
        }

        public void Transfer(string newLocationId)
        {
            LocationId = newLocationId;
        }

        public bool IsExpired()
        {
            return DateTime.UtcNow >= ExpirationDate;
        }

        public bool IsExpiringSoon(int daysThreshold)
        {
            var daysUntilExpiration = (ExpirationDate - DateTime.UtcNow).TotalDays;
            return daysUntilExpiration <= daysThreshold && daysUntilExpiration > 0;
        }

        public int GetDaysUntilExpiration()
        {
            var daysRemaining = (int)(ExpirationDate - DateTime.UtcNow).TotalDays;
            return daysRemaining > 0 ? daysRemaining : 0;
        }

        public decimal GetTotalValue()
        {
            return (decimal)RemainingQuantity * UnitCost;
        }
    }
}
