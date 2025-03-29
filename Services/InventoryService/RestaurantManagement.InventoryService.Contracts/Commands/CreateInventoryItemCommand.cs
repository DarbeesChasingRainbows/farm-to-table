using System;

namespace RestaurantManagement.InventoryService.Contracts.Commands
{
    public class CreateInventoryItemCommand
    {
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
        public string[] AlternativeItemIds { get; set; }
    }

    public class CreateInventoryItemResult
    {
        public string ItemId { get; set; }
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }
    }
}
