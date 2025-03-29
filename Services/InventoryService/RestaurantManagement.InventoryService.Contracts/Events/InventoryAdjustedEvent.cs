using System;
using System.Collections.Generic;

namespace RestaurantManagement.InventoryService.Contracts.Events
{
    public class InventoryAdjustedEvent
    {
        public string Id { get; set; }
        public string TransactionId { get; set; }
        public List<AdjustedItem> Items { get; set; }
        public DateTime Timestamp { get; set; }
        public string UserId { get; set; }
        public string Notes { get; set; }
    }

    public class AdjustedItem
    {
        public string ItemId { get; set; }
        public string ItemName { get; set; }
        public string LocationId { get; set; }
        public string LocationName { get; set; }
        public double PreviousQuantity { get; set; }
        public double NewQuantity { get; set; }
        public double Variance { get; set; }
        public string ReasonCode { get; set; }
        public string BatchId { get; set; }
        public decimal EstimatedValue { get; set; }
    }
}
