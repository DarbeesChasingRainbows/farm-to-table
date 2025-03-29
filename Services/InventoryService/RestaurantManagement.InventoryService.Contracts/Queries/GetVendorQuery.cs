using System;

namespace RestaurantManagement.InventoryService.Contracts.Queries
{
    public class GetVendorQuery
    {
        public string VendorId { get; set; }
        public bool IncludeItems { get; set; } = true;
    }
}
