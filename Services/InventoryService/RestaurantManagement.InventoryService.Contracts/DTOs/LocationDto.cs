using System;

namespace RestaurantManagement.InventoryService.Contracts.DTOs
{
    public class LocationDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public StorageConditionDto StorageConditions { get; set; }
        public double? Capacity { get; set; }
        public string CapacityUnit { get; set; }
        public bool IsActive { get; set; }
    }

    public class StorageConditionDto
    {
        public double? MinTemperature { get; set; }
        public double? MaxTemperature { get; set; }
        public double? TargetHumidity { get; set; }
        public string SpecialRequirements { get; set; }
    }
}
