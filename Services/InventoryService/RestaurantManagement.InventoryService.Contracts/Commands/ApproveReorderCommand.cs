using System;
using System.Collections.Generic;

namespace RestaurantManagement.InventoryService.Contracts.Commands
{
    public class ApproveReorderCommand
    {
        public List<VendorOrderCommand> VendorOrders { get; set; }
        public string ApprovedBy { get; set; }
        public DateTime ApprovalDate { get; set; }
    }

    public class VendorOrderCommand
    {
        public string VendorId { get; set; }
        public List<ReorderItemCommand> Items { get; set; }
        public DateTime RequestedDeliveryDate { get; set; }
        public string Notes { get; set; }
    }

    public class ReorderItemCommand
    {
        public string ItemId { get; set; }
        public double OrderQuantity { get; set; }
        public decimal UnitPrice { get; set; }
    }

    public class ApproveReorderResult
    {
        public List<string> PurchaseOrderIds { get; set; }
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }
    }
}
