using System;
using System.Collections.Generic;

namespace RestaurantManagement.InventoryService.Contracts.Queries
{
    public class GetLocationsQuery
    {
        public List<string> LocationTypes { get; set; }
        public bool IncludeInactive { get; set; } = false;
        public string Search { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }
}
