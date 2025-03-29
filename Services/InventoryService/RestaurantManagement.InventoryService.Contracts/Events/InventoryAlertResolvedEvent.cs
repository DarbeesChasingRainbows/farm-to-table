using System;

namespace RestaurantManagement.InventoryService.Contracts.Events
{
    public class InventoryAlertResolvedEvent
    {
        public string Id { get; set; }
        public string AlertId { get; set; }
        public string Type { get; set; }
        public DateTime ResolvedAt { get; set; }
        public string ResolvedBy { get; set; }
        public string ResolutionNotes { get; set; }
    }
}
