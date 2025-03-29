using System;

namespace RestaurantManagement.InventoryService.Contracts.Commands
{
    public class CreateInventoryLocationCommand
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public StorageCondition StorageConditions { get; set; }
        public double? Capacity { get; set; }
        public string CapacityUnit { get; set; }
    }

    public class StorageCondition
    {
        public double? MinTemperature { get; set; }
        public double? MaxTemperature { get; set; }
        public double? TargetHumidity { get; set; }
        public string SpecialRequirements { get; set; }
    }

    public class CreateInventoryLocationResult
    {
        public string LocationId { get; set; }
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }
    }
}
