using System;
using System.Collections.Generic;

namespace RestaurantManagement.InventoryService.Contracts.Commands
{
    public class RecordWasteCommand
    {
        public List<WasteItemCommand> Items { get; set; }
        public DateTime WasteDate { get; set; }
        public string UserId { get; set; }
        public string WasteReason { get; set; }
        public string Notes { get; set; }
    }

    public class WasteItemCommand
    {
        public string ItemId { get; set; }
        public double Quantity { get; set; }
        public string LocationId { get; set; }
        public string BatchId { get; set; }
        public decimal CostPerUnit { get; set; }
    }

    public class RecordWasteResult
    {
        public string TransactionId { get; set; }
        public decimal TotalWasteCost { get; set; }
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }
    }
}
