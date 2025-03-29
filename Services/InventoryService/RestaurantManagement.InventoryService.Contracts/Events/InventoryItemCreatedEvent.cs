using System;

namespace RestaurantManagement.InventoryService.Contracts.Events
{
    public class InventoryItemCreatedEvent
    {
        public string Id { get; set; }
        public string ItemId { get; set; }
        public string Name { get; set; }
        public string SKU { get; set; }
        public string Category { get; set; }
        public string Subcategory { get; set; }
        public string UnitOfMeasure { get; set; }
        public DateTime Timestamp { get; set; }
        public string UserId { get; set; }
    }
}
