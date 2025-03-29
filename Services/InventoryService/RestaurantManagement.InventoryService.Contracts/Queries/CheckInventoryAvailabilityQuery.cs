using System;
using System.Collections.Generic;

namespace RestaurantManagement.InventoryService.Contracts.Queries
{
    public class CheckInventoryAvailabilityQuery
    {
        public List<InventoryItemCheck> Items { get; set; }
        public string LocationId { get; set; }
    }

    public class InventoryItemCheck
    {
        public string ItemId { get; set; }
        public double RequiredQuantity { get; set; }
    }
}
