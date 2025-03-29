using System;
using System.Collections.Generic;

namespace RestaurantManagement.InventoryService.Contracts.Events
{
    public class InventoryReceivedEvent
    {
        public string Id { get; set; }
        public string TransactionId { get; set; }
        public string PurchaseOrderId { get; set; }
        public List<ReceivedItem> Items { get; set; }
        public DateTime Timestamp { get; set; }
        public string UserId { get; set; }
    }

    public class ReceivedItem
    {
        public string ItemId { get; set; }
        public string ItemName { get; set; }
        public double Quantity { get; set; }
        public string UnitOfMeasure { get; set; }
        public string LocationId { get; set; }
        public string LocationName { get; set; }
        public decimal UnitCost { get; set; }
        public string BatchNumber { get; set; }
        public DateTime? ExpirationDate { get; set; }
    }
}
