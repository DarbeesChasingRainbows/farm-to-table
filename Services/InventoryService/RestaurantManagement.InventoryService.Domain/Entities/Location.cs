using System;

namespace RestaurantManagement.InventoryService.Domain.Entities
{
    public class Location
    {
        // Required by EF Core
        protected Location() { }

        public Location(string id, string name, string type)
        {
            Id = id;
            Name = name;
            Type = type;
            IsActive = true;
            CreatedAt = DateTime.UtcNow;
        }

        public string Id { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public string Type { get; private set; }
        public double? MinTemperature { get; private set; }
        public double? MaxTemperature { get; private set; }
        public double? TargetHumidity { get; private set; }
        public string SpecialRequirements { get; private set; }
        public double? Capacity { get; private set; }
        public string CapacityUnit { get; private set; }
        public bool IsActive { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public string CreatedBy { get; private set; }
        public DateTime? LastModifiedAt { get; private set; }
        public string LastModifiedBy { get; private set; }

        public void UpdateDetails(string name, string description, string type, string modifiedBy)
        {
            Name = name;
            Description = description;
            Type = type;
            LastModifiedBy = modifiedBy;
            LastModifiedAt = DateTime.UtcNow;
        }

        public void UpdateStorageConditions(
            double? minTemperature,
            double? maxTemperature,
            double? targetHumidity,
            string specialRequirements,
            string modifiedBy
        )
        {
            MinTemperature = minTemperature;
            MaxTemperature = maxTemperature;
            TargetHumidity = targetHumidity;
            SpecialRequirements = specialRequirements;
            LastModifiedBy = modifiedBy;
            LastModifiedAt = DateTime.UtcNow;
        }

        public void UpdateCapacity(double? capacity, string capacityUnit, string modifiedBy)
        {
            Capacity = capacity;
            CapacityUnit = capacityUnit;
            LastModifiedBy = modifiedBy;
            LastModifiedAt = DateTime.UtcNow;
        }

        public void Deactivate(string modifiedBy)
        {
            IsActive = false;
            LastModifiedBy = modifiedBy;
            LastModifiedAt = DateTime.UtcNow;
        }

        public void Activate(string modifiedBy)
        {
            IsActive = true;
            LastModifiedBy = modifiedBy;
            LastModifiedAt = DateTime.UtcNow;
        }
    }
}
