using System;

namespace RestaurantManagement.InventoryService.Contracts.Commands
{
    public class CreateBatchCommand
    {
        public string ItemId { get; set; }
        public string BatchNumber { get; set; }
        public DateTime ReceivedDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public double Quantity { get; set; }
        public decimal CostPerUnit { get; set; }
        public string LocationId { get; set; }
        public string VendorId { get; set; }
        public string PurchaseOrderId { get; set; }
    }

    public class CreateBatchResult
    {
        public string BatchId { get; set; }
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }
    }
}
