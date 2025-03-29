using System;

namespace RestaurantManagement.InventoryService.Contracts.Events
{
    public class BatchExpiredEvent
    {
        public string Id { get; set; }
        public string BatchId { get; set; }
        public string BatchNumber { get; set; }
        public string ItemId { get; set; }
        public string ItemName { get; set; }
        public string LocationId { get; set; }
        public string LocationName { get; set; }
        public DateTime ExpirationDate { get; set; }
        public double RemainingQuantity { get; set; }
        public string UnitOfMeasure { get; set; }
        public decimal EstimatedValue { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
