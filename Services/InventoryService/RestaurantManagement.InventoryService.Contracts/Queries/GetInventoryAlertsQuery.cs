using System;
using System.Collections.Generic;

namespace RestaurantManagement.InventoryService.Contracts.Queries
{
    public class GetInventoryAlertsQuery
    {
        public List<string> AlertTypes { get; set; }
        public List<string> Severities { get; set; }
        public string ItemId { get; set; }
        public string LocationId { get; set; }
        public bool IncludeResolved { get; set; } = false;
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
        public string SortBy { get; set; } = "Timestamp";
        public bool SortDescending { get; set; } = true;
    }
}
