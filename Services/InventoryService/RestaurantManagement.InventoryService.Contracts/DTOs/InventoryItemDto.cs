using System;
using System.Collections.Generic;

namespace RestaurantManagement.InventoryService.Contracts.DTOs
{
    public class InventoryItemDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string SKU { get; set; }
        public string Category { get; set; }
        public string Subcategory { get; set; }
        public string UnitOfMeasure { get; set; }
        public string StorageRequirements { get; set; }
        public double ReorderThreshold { get; set; }
        public double MinStockLevel { get; set; }
        public double MaxStockLevel { get; set; }
        public int LeadTimeDays { get; set; }
        public bool TrackExpiration { get; set; }
        public string DefaultVendorId { get; set; }
        public List<string> AlternativeItemIds { get; set; }
        public decimal AverageCost { get; set; }
        public decimal LastCost { get; set; }
        public List<StockLevelDto> StockLevels { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? LastModifiedAt { get; set; }
        public string LastModifiedBy { get; set; }
    }
}
