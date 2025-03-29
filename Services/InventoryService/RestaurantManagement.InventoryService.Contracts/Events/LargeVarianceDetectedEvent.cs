using System;

namespace RestaurantManagement.InventoryService.Contracts.Events
{
    public class LargeVarianceDetectedEvent
    {
        public string Id { get; set; }
        public string CountSheetId { get; set; }
        public string ItemId { get; set; }
        public string ItemName { get; set; }
        public string LocationId { get; set; }
        public string LocationName { get; set; }
        public double SystemQuantity { get; set; }
        public double CountedQuantity { get; set; }
        public double Variance { get; set; }
        public double VariancePercentage { get; set; }
        public decimal VarianceValue { get; set; }
        public string UnitOfMeasure { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
