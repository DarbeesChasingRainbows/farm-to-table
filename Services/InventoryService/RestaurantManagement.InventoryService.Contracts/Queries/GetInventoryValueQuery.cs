using System;
using System.Collections.Generic;

namespace RestaurantManagement.InventoryService.Contracts.Queries
{
    public class GetInventoryValueQuery
    {
        public List<string> LocationIds { get; set; }
        public List<string> CategoryIds { get; set; }
        public DateTime? AsOfDate { get; set; }
    }
}
