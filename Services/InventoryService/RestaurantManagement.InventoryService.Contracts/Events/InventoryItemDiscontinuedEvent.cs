using System;

namespace RestaurantManagement.InventoryService.Contracts.Events
{
    public class InventoryItemDiscontinuedEvent
    {
        public string Id { get; set; }
        public string ItemId { get; set; }
        public string Name { get; set; }
        public string SKU { get; set; }
        public string Reason { get; set; }
        public DateTime Timestamp { get; set; }
        public string UserId { get; set; }
    }
}
