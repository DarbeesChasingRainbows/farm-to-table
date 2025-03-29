using System;
using System.Collections.Generic;

namespace RestaurantManagement.InventoryService.Contracts.Queries
{
    public class GetStockLevelsQuery
    {
        public List<string> ItemIds { get; set; }
        public string LocationId { get; set; }
        public bool BelowThreshold { get; set; } = false;
        public bool IncludeZeroStock { get; set; } = true;
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }
}
