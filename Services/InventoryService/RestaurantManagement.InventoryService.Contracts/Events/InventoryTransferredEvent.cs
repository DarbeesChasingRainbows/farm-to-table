using System;
using System.Collections.Generic;

namespace RestaurantManagement.InventoryService.Contracts.Events
{
    public class InventoryTransferredEvent
    {
        public string Id { get; set; }
        public string TransactionId { get; set; }
        public List<TransferredItem> Items { get; set; }
        public DateTime Timestamp { get; set; }
        public string UserId { get; set; }
    }

    public class TransferredItem
    {
        public string ItemId { get; set; }
        public string ItemName { get; set; }
        public double Quantity { get; set; }
        public string UnitOfMeasure { get; set; }
        public string SourceLocationId { get; set; }
        public string SourceLocationName { get; set; }
        public string DestinationLocationId { get; set; }
        public string DestinationLocationName { get; set; }
        public List<TransferredBatch> Batches { get; set; }
    }

    public class TransferredBatch
    {
        public string BatchId { get; set; }
        public string BatchNumber { get; set; }
        public double Quantity { get; set; }
    }
}
