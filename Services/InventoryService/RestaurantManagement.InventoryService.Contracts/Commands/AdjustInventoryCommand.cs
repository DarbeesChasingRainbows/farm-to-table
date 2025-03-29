using System;
using System.Collections.Generic;

namespace RestaurantManagement.InventoryService.Contracts.Commands
{
    public class AdjustInventoryCommand
    {
        public List<AdjustInventoryItemCommand> Items { get; set; }
        public DateTime AdjustmentDate { get; set; }
        public string UserId { get; set; }
        public string Notes { get; set; }
    }

    public class AdjustInventoryItemCommand
    {
        public string ItemId { get; set; }
        public string LocationId { get; set; }
        public double CurrentQuantity { get; set; }
        public double NewQuantity { get; set; }
        public string ReasonCode { get; set; }
        public string BatchId { get; set; }
    }

    public class AdjustInventoryResult
    {
        public string TransactionId { get; set; }
        public List<InventoryAdjustment> Adjustments { get; set; }
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }
    }

    public class InventoryAdjustment
    {
        public string ItemId { get; set; }
        public string ItemName { get; set; }
        public string LocationId { get; set; }
        public double PreviousQuantity { get; set; }
        public double NewQuantity { get; set; }
        public double Variance { get; set; }
    }
}
