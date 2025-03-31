using System;

namespace RestaurantManagement.InventoryService.Domain.Events
{
    public abstract class LocationEvent : BaseDomainEvent
    {
        public string LocationId { get; }

        protected LocationEvent(string locationId, string userId)
            : base(userId)
        {
            LocationId = locationId ?? throw new ArgumentNullException(nameof(locationId));
        }
    }

    public class LocationCreatedEvent : LocationEvent
    {
        public string Name { get; }
        public string Type { get; }

        public LocationCreatedEvent(string locationId, string name, string type, string userId)
            : base(locationId, userId)
        {
            Name = name;
            Type = type;
        }
    }

    public class LocationDetailsUpdatedEvent : LocationEvent
    {
        public string Name { get; }
        public string Description { get; }
        public string Type { get; }

        public LocationDetailsUpdatedEvent(
            string locationId,
            string name,
            string description,
            string type,
            string userId
        )
            : base(locationId, userId)
        {
            Name = name;
            Description = description;
            Type = type;
        }
    }

    public class LocationStorageConditionsUpdatedEvent : LocationEvent
    {
        public double? MinTemperature { get; }
        public double? MaxTemperature { get; }
        public double? TargetHumidity { get; }
        public string SpecialRequirements { get; }

        public LocationStorageConditionsUpdatedEvent(
            string locationId,
            double? minTemperature,
            double? maxTemperature,
            double? targetHumidity,
            string specialRequirements,
            string userId
        )
            : base(locationId, userId)
        {
            MinTemperature = minTemperature;
            MaxTemperature = maxTemperature;
            TargetHumidity = targetHumidity;
            SpecialRequirements = specialRequirements;
        }
    }

    public class LocationCapacityUpdatedEvent : LocationEvent
    {
        public double? Capacity { get; }
        public string CapacityUnit { get; }

        public LocationCapacityUpdatedEvent(
            string locationId,
            double? capacity,
            string capacityUnit,
            string userId
        )
            : base(locationId, userId)
        {
            Capacity = capacity;
            CapacityUnit = capacityUnit;
        }
    }

    public class LocationDeactivatedEvent : LocationEvent
    {
        public LocationDeactivatedEvent(string locationId, string userId)
            : base(locationId, userId) { }
    }

    public class LocationActivatedEvent : LocationEvent
    {
        public LocationActivatedEvent(string locationId, string userId)
            : base(locationId, userId) { }
    }
}
