using System;
using System.Collections.Generic;

namespace RestaurantManagement.InventoryService.Contracts.Commands
{
    public class ReceiveInventoryCommand
    {
        public string PurchaseOrderId { get; set; }
        public DateTime ReceivedDate { get; set; }
        public List<ReceiveInventoryItemCommand> Items { get; set; }
        public string UserId { get; set; }
        public string Notes { get; set; }
    }

    public class ReceiveInventoryItemCommand
    {
        public string ItemId { get; set; }
        public double Quantity { get; set; }
        public string UnitOfMeasure { get; set; }
        public string LocationId { get; set; }
        public decimal UnitCost { get; set; }
        public string BatchNumber { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public string VendorItemCode { get; set; }
        public string DiscrepancyReason { get; set; }
    }

    public class ReceiveInventoryResult
    {
        public string TransactionId { get; set; }
        public List<UpdatedStockLevel> UpdatedStockLevels { get; set; }
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }
    }

    public class UpdatedStockLevel
    {
        public string ItemId { get; set; }
        public string LocationId { get; set; }
        public double CurrentQuantity { get; set; }
        public double PreviousQuantity { get; set; }
    }
}
