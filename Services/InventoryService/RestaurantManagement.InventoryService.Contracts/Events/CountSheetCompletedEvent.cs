using System;

namespace RestaurantManagement.InventoryService.Contracts.Events
{
    public class CountSheetCompletedEvent
    {
        public string Id { get; set; }
        public string CountSheetId { get; set; }
        public string LocationId { get; set; }
        public string LocationName { get; set; }
        public int TotalItems { get; set; }
        public int VarianceItems { get; set; }
        public double TotalVariancePercentage { get; set; }
        public decimal TotalVarianceValue { get; set; }
        public DateTime CompletedDate { get; set; }
        public string CountedBy { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
