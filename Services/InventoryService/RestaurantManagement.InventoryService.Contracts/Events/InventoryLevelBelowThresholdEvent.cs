using System;

namespace RestaurantManagement.InventoryService.Contracts.Events
{
    public class InventoryLevelBelowThresholdEvent
    {
        public string Id { get; set; }
        public string ItemId { get; set; }
        public string ItemName { get; set; }
        public string LocationId { get; set; }
        public string LocationName { get; set; }
        public double CurrentLevel { get; set; }
        public double ThresholdLevel { get; set; }
        public string UnitOfMeasure { get; set; }
        public double SuggestedOrderQuantity { get; set; }
        public int LeadTimeDays { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
