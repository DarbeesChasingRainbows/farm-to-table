using System;
using System.Collections.Generic;

namespace RestaurantManagement.InventoryService.Contracts.Queries
{
    public class GetInventoryTransactionsQuery
    {
        public string ItemId { get; set; }
        public string LocationId { get; set; }
        public List<string> TransactionTypes { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string ReferenceId { get; set; }
        public string ReferenceType { get; set; }
        public string UserId { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
        public string SortBy { get; set; } = "Timestamp";
        public bool SortDescending { get; set; } = true;
    }
}
