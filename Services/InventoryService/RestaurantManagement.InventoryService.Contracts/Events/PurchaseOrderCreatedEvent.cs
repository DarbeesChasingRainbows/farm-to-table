using System;
using System.Collections.Generic;

namespace RestaurantManagement.InventoryService.Contracts.Events
{
    public class PurchaseOrderCreatedEvent
    {
        public string Id { get; set; }
        public string PurchaseOrderId { get; set; }
        public string VendorId { get; set; }
        public string VendorName { get; set; }
        public int ItemCount { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime ExpectedDeliveryDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
