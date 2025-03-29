using System;

namespace RestaurantManagement.InventoryService.Contracts.Queries
{
    public class GetVendorItemsQuery
    {
        public string VendorId { get; set; }
        public string Search { get; set; }
        public string Category { get; set; }
        public bool OnlyPreferred { get; set; } = false;
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }
}
