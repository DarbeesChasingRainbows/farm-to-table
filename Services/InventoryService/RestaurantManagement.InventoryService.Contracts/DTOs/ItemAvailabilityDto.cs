using System;
using System.Collections.Generic;

namespace RestaurantManagement.InventoryService.Contracts.DTOs
{
    public class ItemAvailabilityDto
    {
        public string ItemId { get; set; }
        public string ItemName { get; set; }
        public bool IsAvailable { get; set; }
        public double AvailableQuantity { get; set; }
        public double RequiredQuantity { get; set; }
        public string UnitOfMeasure { get; set; }
        public List<LocationAvailabilityDto> AvailabilityByLocation { get; set; }
    }

    public class LocationAvailabilityDto
    {
        public string LocationId { get; set; }
        public string LocationName { get; set; }
        public double AvailableQuantity { get; set; }
    }

    public class AvailabilityCheckResultDto
    {
        public bool AllItemsAvailable { get; set; }
        public List<ItemAvailabilityDto> ItemAvailability { get; set; }
    }
}
