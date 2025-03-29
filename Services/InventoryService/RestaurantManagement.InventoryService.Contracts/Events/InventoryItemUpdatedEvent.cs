using System;

namespace RestaurantManagement.InventoryService.Contracts.Events
{
    public class InventoryItemUpdatedEvent
    {
        public string Id { get; set; }
        public string ItemId { get; set; }
        public string Name { get; set; }
        public string SKU { get; set; }
        public DateTime Timestamp { get; set; }
        public string UserId { get; set; }
        public string[] ChangedProperties { get; set; }
    }
}
