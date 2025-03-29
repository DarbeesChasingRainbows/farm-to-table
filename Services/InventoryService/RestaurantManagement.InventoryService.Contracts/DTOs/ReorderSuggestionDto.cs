using System;
using System.Collections.Generic;

namespace RestaurantManagement.InventoryService.Contracts.DTOs
{
    public class ReorderSuggestionDto
    {
        public string ItemId { get; set; }
        public string ItemName { get; set; }
        public string SKU { get; set; }
        public string VendorId { get; set; }
        public string VendorName { get; set; }
        public double CurrentStock { get; set; }
        public double ReorderThreshold { get; set; }
        public double SuggestedOrderQuantity { get; set; }
        public string UnitOfMeasure { get; set; }
        public decimal EstimatedUnitCost { get; set; }
        public decimal EstimatedTotalCost { get; set; }
        public int LeadTimeDays { get; set; }
        public bool IsSelected { get; set; }
        public double ActualOrderQuantity { get; set; }
    }

    public class VendorOrderSuggestionDto
    {
        public string VendorId { get; set; }
        public string VendorName { get; set; }
        public List<ReorderSuggestionDto> Items { get; set; }
        public decimal TotalEstimatedCost { get; set; }
        public int ItemCount { get; set; }
    }
}
