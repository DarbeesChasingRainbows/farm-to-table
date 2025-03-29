using System;

namespace RestaurantManagement.InventoryService.Contracts.Queries
{
    public class GetExpiringInventoryQuery
    {
        public int DaysToExpiration { get; set; } = 30;
        public string LocationId { get; set; }
        public string CategoryId { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }
}
