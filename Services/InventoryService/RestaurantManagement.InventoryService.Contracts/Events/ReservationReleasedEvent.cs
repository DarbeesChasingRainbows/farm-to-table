using System;
using System.Collections.Generic;

namespace RestaurantManagement.InventoryService.Contracts.Events
{
    public class ReservationReleasedEvent
    {
        public string Id { get; set; }
        public string ReservationId { get; set; }
        public string ReferenceId { get; set; }
        public List<ReleasedItem> Items { get; set; }
        public DateTime Timestamp { get; set; }
    }

    public class ReleasedItem
    {
        public string ItemId { get; set; }
        public string ItemName { get; set; }
        public double Quantity { get; set; }
        public string UnitOfMeasure { get; set; }
        public string LocationId { get; set; }
        public string LocationName { get; set; }
    }
}
