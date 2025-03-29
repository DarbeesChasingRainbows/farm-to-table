using System;
using System.Collections.Generic;

namespace RestaurantManagement.InventoryService.Contracts.Queries
{
    public class GetInventoryUsageQuery
    {
        public List<string> ItemIds { get; set; }
        public string CategoryId { get; set; }
        public string LocationId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string GroupBy { get; set; } = "Day";  // Day, Week, Month, Quarter, Year
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }
}
