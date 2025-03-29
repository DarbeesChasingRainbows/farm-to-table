using System;
using System.Collections.Generic;

namespace RestaurantManagement.InventoryService.Contracts.Queries
{
    public class GetCountSheetsQuery
    {
        public string LocationId { get; set; }
        public List<string> Statuses { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string RequestedBy { get; set; }
        public string CountedBy { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
        public string SortBy { get; set; } = "CountDate";
        public bool SortDescending { get; set; } = true;
    }
}
