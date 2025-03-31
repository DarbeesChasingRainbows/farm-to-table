using System;
using System.Collections.Generic;

namespace RestaurantManagement.InventoryService.Domain.Events
{
    public abstract class InventoryTransactionEvent : BaseDomainEvent
    {
        public string TransactionId { get; }

        protected InventoryTransactionEvent(string transactionId, string userId)
            : base(userId)
        {
            TransactionId = transactionId ?? throw new ArgumentNullException(nameof(transactionId));
        }
    }

    public class InventoryTransactionCreatedEvent : InventoryTransactionEvent
    {
        public string Type { get; }
        public DateTime TransactionDate { get; }

        public InventoryTransactionCreatedEvent(
            string transactionId,
            string type,
            DateTime transactionDate,
            string userId
        )
            : base(transactionId, userId)
        {
            Type = type;
            TransactionDate = transactionDate;
        }
    }

    public class InventoryTransactionReferenceSetEvent : InventoryTransactionEvent
    {
        public string ReferenceNumber { get; }
        public string ReferenceType { get; }

        public InventoryTransactionReferenceSetEvent(
            string transactionId,
            string referenceNumber,
            string referenceType,
            string userId
        )
            : base(transactionId, userId)
        {
            ReferenceNumber = referenceNumber;
            ReferenceType = referenceType;
        }
    }

    public class InventoryTransactionLocationSetEvent : InventoryTransactionEvent
    {
        public string LocationId { get; }

        public InventoryTransactionLocationSetEvent(
            string transactionId,
            string locationId,
            string userId
        )
            : base(transactionId, userId)
        {
            LocationId = locationId;
        }
    }

    public class InventoryTransactionDestinationLocationSetEvent : InventoryTransactionEvent
    {
        public string DestinationLocationId { get; }

        public InventoryTransactionDestinationLocationSetEvent(
            string transactionId,
            string destinationLocationId,
            string userId
        )
            : base(transactionId, userId)
        {
            DestinationLocationId = destinationLocationId;
        }
    }

    public class InventoryTransactionNotesSetEvent : InventoryTransactionEvent
    {
        public string Notes { get; }

        public InventoryTransactionNotesSetEvent(string transactionId, string notes, string userId)
            : base(transactionId, userId)
        {
            Notes = notes;
        }
    }

    public class InventoryTransactionItemAddedEvent : InventoryTransactionEvent
    {
        public string TransactionItemId { get; }
        public string ItemId { get; }
        public double Quantity { get; }
        public string LocationId { get; }
        public string BatchId { get; }
        public decimal? UnitCost { get; }

        public InventoryTransactionItemAddedEvent(
            string transactionId,
            string transactionItemId,
            string itemId,
            double quantity,
            string locationId,
            string batchId,
            decimal? unitCost,
            string userId
        )
            : base(transactionId, userId)
        {
            TransactionItemId = transactionItemId;
            ItemId = itemId;
            Quantity = quantity;
            LocationId = locationId;
            BatchId = batchId;
            UnitCost = unitCost;
        }
    }

    public class InventoryTransactionCompletedEvent : InventoryTransactionEvent
    {
        public List<TransactionItemSummary> Items { get; } = new List<TransactionItemSummary>();

        public InventoryTransactionCompletedEvent(
            string transactionId,
            List<TransactionItemSummary> items,
            string userId
        )
            : base(transactionId, userId)
        {
            Items = items;
        }
    }

    public class TransactionItemSummary
    {
        public string ItemId { get; }
        public string ItemName { get; }
        public double Quantity { get; }
        public string LocationId { get; }
        public string BatchId { get; }
        public decimal? UnitCost { get; }

        public TransactionItemSummary(
            string itemId,
            string itemName,
            double quantity,
            string locationId,
            string batchId = null,
            decimal? unitCost = null
        )
        {
            ItemId = itemId;
            ItemName = itemName;
            Quantity = quantity;
            LocationId = locationId;
            BatchId = batchId;
            UnitCost = unitCost;
        }
    }
}
