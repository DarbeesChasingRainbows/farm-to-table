using System;
using System.Collections.Generic;

namespace RestaurantManagement.InventoryService.Contracts.Events
{
    public class CountSheetCreatedEvent
    {
        public string Id { get; set; }
        public string CountSheetId { get; set; }
        public string LocationId { get; set; }
        public string LocationName { get; set; }
        public List<string> Categories { get; set; }
        public int ItemCount { get; set; }
        public DateTime CountDate { get; set; }
        public string RequestedBy { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
