using System;

namespace RestaurantManagement.InventoryService.Contracts.DTOs
{
    public class StockLevelDto
    {
        public string ItemId { get; set; }
        public string LocationId { get; set; }
        public string LocationName { get; set; }
        public double CurrentQuantity { get; set; }
        public double ReservedQuantity { get; set; }
        public double AvailableQuantity { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}
