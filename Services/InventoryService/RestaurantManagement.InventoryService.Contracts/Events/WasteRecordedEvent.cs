using System;
using System.Collections.Generic;

namespace RestaurantManagement.InventoryService.Contracts.Events
{
    public class WasteRecordedEvent
    {
        public string Id { get; set; }
        public string TransactionId { get; set; }
        public List<WastedItem> Items { get; set; }
        public DateTime Timestamp { get; set; }
        public string UserId { get; set; }
        public string WasteReason { get; set; }
        public decimal TotalWasteCost { get; set; }
    }

    public class WastedItem
    {
        public string ItemId { get; set; }
        public string ItemName { get; set; }
        public double Quantity { get; set; }
        public string UnitOfMeasure { get; set; }
        public string LocationId { get; set; }
        public string LocationName { get; set; }
        public string BatchId { get; set; }
        public decimal CostPerUnit { get; set; }
        public decimal TotalCost { get; set; }
    }
}
