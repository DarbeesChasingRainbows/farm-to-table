using System;

namespace RestaurantManagement.InventoryService.Contracts.Queries
{
    public class GetInventoryReservationsQuery
    {
        public string ReferenceId { get; set; }
        public string ItemId { get; set; }
        public string LocationId { get; set; }
        public bool IncludeExpired { get; set; } = false;
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }
}
