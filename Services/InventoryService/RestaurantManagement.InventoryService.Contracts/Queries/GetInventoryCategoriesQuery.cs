using System;

namespace RestaurantManagement.InventoryService.Contracts.Queries
{
    public class GetInventoryCategoriesQuery
    {
        public bool IncludeInactive { get; set; } = false;
        public bool IncludeEmpty { get; set; } = false;
    }
}
