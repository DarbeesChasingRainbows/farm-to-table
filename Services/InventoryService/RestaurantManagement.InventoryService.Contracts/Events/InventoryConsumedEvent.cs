using System;
using System.Collections.Generic;

namespace RestaurantManagement.InventoryService.Contracts.Events
{
    public class InventoryConsumedEvent
    {
        public string Id { get; set; }
        public string TransactionId { get; set; }
        public string ReferenceType { get; set; }
        public string ReferenceId { get; set; }
        public List<ConsumedItem> Items { get; set; }
        public DateTime Timestamp { get; set; }
        public string UserId { get; set; }
    }

    public class ConsumedItem
    {
        public string ItemId { get; set; }
        public string ItemName { get; set; }
        public double Quantity { get; set; }
        public string UnitOfMeasure { get; set; }
        public string LocationId { get; set; }
        public string LocationName { get; set; }
        public List<ConsumedBatch> Batches { get; set; }
    }

    public class ConsumedBatch
    {
        public string BatchId { get; set; }
        public string BatchNumber { get; set; }
        public double Quantity { get; set; }
    }
}
