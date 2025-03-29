using System;

namespace RestaurantManagement.InventoryService.Contracts.DTOs
{
    public class InventoryItemBasicDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string SKU { get; set; }
        public string Category { get; set; }
        public string UnitOfMeasure { get; set; }
        public bool IsActive { get; set; }
    }
}
