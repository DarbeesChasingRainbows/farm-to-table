using System;
using System.Collections.Generic;

namespace RestaurantManagement.InventoryService.Contracts.Queries
{
    public class GetInventoryItemsQuery
    {
        public string Search { get; set; }
        public string Category { get; set; }
        public string Subcategory { get; set; }
        public string LocationId { get; set; }
        public bool IncludeInactive { get; set; } = false;
        public bool IncludeZeroStock { get; set; } = true;
        public bool BelowThreshold { get; set; } = false;
        public List<string> ItemIds { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
        public string SortBy { get; set; } = "Name";
        public bool SortDescending { get; set; } = false;
    }
}
