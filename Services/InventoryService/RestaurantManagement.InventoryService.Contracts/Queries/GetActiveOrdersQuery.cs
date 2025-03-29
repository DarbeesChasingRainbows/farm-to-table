using System;
using System.Collections.Generic;

namespace RestaurantManagement.InventoryService.Contracts.Queries
{
    public class GetActiveOrdersQuery
    {
        public string OrderType { get; set; }
        public List<string> OrderStatuses { get; set; }
        public string LocationId { get; set; }
        public bool IncludeOrderItems { get; set; } = true;
        public bool IncludeCustomerDetails { get; set; } = false;
        public bool IncludePayments { get; set; } = false;
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }
}
