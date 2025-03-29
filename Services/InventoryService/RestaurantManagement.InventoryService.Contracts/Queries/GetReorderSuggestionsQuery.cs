using System;
using System.Collections.Generic;

namespace RestaurantManagement.InventoryService.Contracts.Queries
{
    public class GetReorderSuggestionsQuery
    {
        public List<string> LocationIds { get; set; }
        public List<string> CategoryIds { get; set; }
        public bool GroupByVendor { get; set; } = true;
    }
}
