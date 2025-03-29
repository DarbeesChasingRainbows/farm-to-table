using System;

namespace RestaurantManagement.InventoryService.Contracts.Queries
{
    public class GetVendorsQuery
    {
        public string Search { get; set; }
        public bool IncludeInactive { get; set; } = false;
        public string ItemId { get; set; }
        public bool OnlyPreferred { get; set; } = false;
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }
}
