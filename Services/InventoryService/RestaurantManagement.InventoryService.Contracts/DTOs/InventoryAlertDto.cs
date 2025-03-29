using System;

namespace RestaurantManagement.InventoryService.Contracts.DTOs
{
    public class InventoryAlertDto
    {
        public string Id { get; set; }
        public string Type { get; set; }
        public string Severity { get; set; }
        public string ItemId { get; set; }
        public string ItemName { get; set; }
        public string LocationId { get; set; }
        public string LocationName { get; set; }
        public string Message { get; set; }
        public DateTime Timestamp { get; set; }
        public bool IsResolved { get; set; }
        public DateTime? ResolvedAt { get; set; }
        public string ResolvedBy { get; set; }
        public string ResolutionNotes { get; set; }
    }
}
