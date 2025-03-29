using System;

namespace RestaurantManagement.InventoryService.Contracts.Events
{
    public class InventoryValueChangedEvent
    {
        public string Id { get; set; }
        public decimal PreviousTotalValue { get; set; }
        public decimal NewTotalValue { get; set; }
        public decimal Change { get; set; }
        public double ChangePercentage { get; set; }
        public DateTime AsOfDate { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
