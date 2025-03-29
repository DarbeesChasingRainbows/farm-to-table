using System;
using System.Collections.Generic;

namespace RestaurantManagement.InventoryService.Domain.Entities
{
    public class InventoryTransaction
    {
        private readonly List<TransactionItem> _items = new List<TransactionItem>();

        // Required by EF Core
        protected InventoryTransaction() { }

        public InventoryTransaction(string id, string type, DateTime transactionDate, string userId)
        {
            Id = id;
            Type = type;
            TransactionDate = transactionDate;
            UserId = userId;
        }

        public string Id { get; private set; }
        public string Type { get; private set; }
        public DateTime TransactionDate { get; private set; }
        public string ReferenceNumber { get; private set; }
        public string ReferenceType { get; private set; }
        public string LocationId { get; private set; }
        public string DestinationLocationId { get; private set; }
        public string UserId { get; private set; }
        public string Notes { get; private set; }

        public IReadOnlyCollection<TransactionItem> Items => _items.AsReadOnly();

        public void SetReference(string referenceNumber, string referenceType)
        {
            ReferenceNumber = referenceNumber;
            ReferenceType = referenceType;
        }

        public void SetLocation(string locationId)
        {
            LocationId = locationId;
        }

        public void SetDestinationLocation(string destinationLocationId)
        {
            DestinationLocationId = destinationLocationId;
        }

        public void SetNotes(string notes)
        {
            Notes = notes;
        }

        public void AddItem(
            string itemId,
            double quantity,
            string locationId,
            string batchId = null,
            decimal? unitCost = null
        )
        {
            if (quantity <= 0)
                return;

            var transactionItem = new TransactionItem(
                Guid.NewGuid().ToString(),
                Id,
                itemId,
                quantity,
                locationId,
                batchId,
                unitCost
            );

            _items.Add(transactionItem);
        }
    }

    public class TransactionItem
    {
        // Required by EF Core
        protected TransactionItem() { }

        public TransactionItem(
            string id,
            string transactionId,
            string inventoryItemId,
            double quantity,
            string locationId,
            string batchId = null,
            decimal? unitCost = null
        )
        {
            Id = id;
            TransactionId = transactionId;
            InventoryItemId = inventoryItemId;
            Quantity = quantity;
            LocationId = locationId;
            BatchId = batchId;
            UnitCost = unitCost;
        }

        public string Id { get; private set; }
        public string TransactionId { get; private set; }
        public string InventoryItemId { get; private set; }
        public double Quantity { get; private set; }
        public string LocationId { get; private set; }
        public string BatchId { get; private set; }
        public decimal? UnitCost { get; private set; }

        // Navigation properties
        public InventoryTransaction Transaction { get; private set; }
        public InventoryItem InventoryItem { get; private set; }
    }
}
