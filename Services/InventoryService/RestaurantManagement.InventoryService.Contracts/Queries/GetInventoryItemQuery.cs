using System;

namespace RestaurantManagement.InventoryService.Contracts.Queries
{
    public class GetInventoryItemQuery
    {
        public string ItemId { get; set; }
        public bool IncludeStockLevels { get; set; } = true;
        public bool IncludeBatches { get; set; } = false;
        public bool IncludeTransactions { get; set; } = false;
        public int? TransactionCount { get; set; }
    }
}
