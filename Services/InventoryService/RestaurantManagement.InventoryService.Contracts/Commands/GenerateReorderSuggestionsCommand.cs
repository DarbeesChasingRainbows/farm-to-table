using System;
using System.Collections.Generic;

namespace RestaurantManagement.InventoryService.Contracts.Commands
{
    public class GenerateReorderSuggestionsCommand
    {
        public List<string> LocationIds { get; set; }
        public List<string> CategoryIds { get; set; }
        public string RequestedBy { get; set; }
    }

    public class GenerateReorderSuggestionsResult
    {
        public List<ReorderSuggestion> Suggestions { get; set; }
        public int TotalItems { get; set; }
        public int TotalVendors { get; set; }
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }
    }

    public class ReorderSuggestion
    {
        public string ItemId { get; set; }
        public string ItemName { get; set; }
        public string VendorId { get; set; }
        public string VendorName { get; set; }
        public double CurrentStock { get; set; }
        public double ReorderThreshold { get; set; }
        public double SuggestedOrderQuantity { get; set; }
        public string UnitOfMeasure { get; set; }
        public decimal EstimatedCost { get; set; }
        public int LeadTimeDays { get; set; }
    }
}
