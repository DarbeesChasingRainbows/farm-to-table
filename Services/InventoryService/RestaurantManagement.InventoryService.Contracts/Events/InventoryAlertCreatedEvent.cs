using System;

namespace RestaurantManagement.InventoryService.Contracts.Events
{
    public class InventoryAlertCreatedEvent
    {
        public string Id { get; set; }
        public string AlertId { get; set; }
        public string Type { get; set; }
        public string Severity { get; set; }
        public string ItemId { get; set; }
        public string ItemName { get; set; }
        public string LocationId { get; set; }
        public string LocationName { get; set; }
        public string Message { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
