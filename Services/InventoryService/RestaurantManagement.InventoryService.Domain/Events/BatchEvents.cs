using System;

namespace RestaurantManagement.InventoryService.Domain.Events
{
    public abstract class BatchEvent : BaseDomainEvent
    {
        public string BatchId { get; }
        public string InventoryItemId { get; }

        protected BatchEvent(string batchId, string inventoryItemId, string userId)
            : base(userId)
        {
            BatchId = batchId ?? throw new ArgumentNullException(nameof(batchId));
            InventoryItemId =
                inventoryItemId ?? throw new ArgumentNullException(nameof(inventoryItemId));
        }
    }

    public class BatchCreatedEvent : BatchEvent
    {
        public string BatchNumber { get; }
        public DateTime ReceivedDate { get; }
        public DateTime ExpirationDate { get; }
        public double InitialQuantity { get; }
        public decimal UnitCost { get; }
        public string LocationId { get; }

        public BatchCreatedEvent(
            string batchId,
            string inventoryItemId,
            string batchNumber,
            DateTime receivedDate,
            DateTime expirationDate,
            double initialQuantity,
            decimal unitCost,
            string locationId,
            string userId
        )
            : base(batchId, inventoryItemId, userId)
        {
            BatchNumber = batchNumber;
            ReceivedDate = receivedDate;
            ExpirationDate = expirationDate;
            InitialQuantity = initialQuantity;
            UnitCost = unitCost;
            LocationId = locationId;
        }
    }

    public class BatchVendorInfoSetEvent : BatchEvent
    {
        public string VendorId { get; }
        public string PurchaseOrderId { get; }

        public BatchVendorInfoSetEvent(
            string batchId,
            string inventoryItemId,
            string vendorId,
            string purchaseOrderId,
            string userId
        )
            : base(batchId, inventoryItemId, userId)
        {
            VendorId = vendorId;
            PurchaseOrderId = purchaseOrderId;
        }
    }

    public class BatchRemainingQuantityUpdatedEvent : BatchEvent
    {
        public double OldRemainingQuantity { get; }
        public double NewRemainingQuantity { get; }

        public BatchRemainingQuantityUpdatedEvent(
            string batchId,
            string inventoryItemId,
            double oldRemainingQuantity,
            double newRemainingQuantity,
            string userId
        )
            : base(batchId, inventoryItemId, userId)
        {
            OldRemainingQuantity = oldRemainingQuantity;
            NewRemainingQuantity = newRemainingQuantity;
        }
    }

    public class BatchConsumedEvent : BatchEvent
    {
        public double QuantityConsumed { get; }
        public double RemainingQuantity { get; }

        public BatchConsumedEvent(
            string batchId,
            string inventoryItemId,
            double quantityConsumed,
            double remainingQuantity,
            string userId
        )
            : base(batchId, inventoryItemId, userId)
        {
            QuantityConsumed = quantityConsumed;
            RemainingQuantity = remainingQuantity;
        }
    }

    public class BatchTransferredEvent : BatchEvent
    {
        public string OldLocationId { get; }
        public string NewLocationId { get; }

        public BatchTransferredEvent(
            string batchId,
            string inventoryItemId,
            string oldLocationId,
            string newLocationId,
            string userId
        )
            : base(batchId, inventoryItemId, userId)
        {
            OldLocationId = oldLocationId;
            NewLocationId = newLocationId;
        }
    }

    public class BatchExpiredEvent : BatchEvent
    {
        public DateTime ExpirationDate { get; }
        public double RemainingQuantity { get; }

        public BatchExpiredEvent(
            string batchId,
            string inventoryItemId,
            DateTime expirationDate,
            double remainingQuantity,
            string userId
        )
            : base(batchId, inventoryItemId, userId)
        {
            ExpirationDate = expirationDate;
            RemainingQuantity = remainingQuantity;
        }
    }

    public class BatchExpiringSoonEvent : BatchEvent
    {
        public DateTime ExpirationDate { get; }
        public int DaysUntilExpiration { get; }
        public double RemainingQuantity { get; }

        public BatchExpiringSoonEvent(
            string batchId,
            string inventoryItemId,
            DateTime expirationDate,
            int daysUntilExpiration,
            double remainingQuantity,
            string userId
        )
            : base(batchId, inventoryItemId, userId)
        {
            ExpirationDate = expirationDate;
            DaysUntilExpiration = daysUntilExpiration;
            RemainingQuantity = remainingQuantity;
        }
    }
}
