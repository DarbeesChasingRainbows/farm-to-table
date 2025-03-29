using System;

namespace RestaurantManagement.InventoryService.Contracts.Queries
{
    public class GetInventoryAuditLogQuery
    {
        public string EntityType { get; set; }  // InventoryItem, StockLevel, Transaction, etc.
        public string EntityId { get; set; }
        public string UserId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }
}
