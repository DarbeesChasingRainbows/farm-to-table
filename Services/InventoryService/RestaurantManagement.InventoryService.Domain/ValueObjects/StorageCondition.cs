using System;
using RestaurantManagement.InventoryService.Domain.Exceptions;

namespace RestaurantManagement.InventoryService.Domain.ValueObjects
{
    public class StorageCondition
    {
        private StorageCondition() { }

        public StorageCondition(
            double? minTemperature = null,
            double? maxTemperature = null,
            double? targetHumidity = null,
            string specialRequirements = null)
        {
            if (minTemperature.HasValue && maxTemperature.HasValue && minTemperature > maxTemperature)
                throw new InventoryDomainException("Minimum temperature cannot be greater than maximum temperature");

            if (targetHumidity.HasValue && (targetHumidity < 0 || targetHumidity > 100))
                throw new InventoryDomainException("Humidity must be between 0 and 100");

            MinTemperature = minTemperature;
            MaxTemperature = maxTemperature;
            TargetHumidity = targetHumidity;
            SpecialRequirements = specialRequirements;
        }

        public double? MinTemperature { get; private set; }
        public double? MaxTemperature { get; private set; }
        public double? TargetHumidity { get; private set; }
        public string SpecialRequirements { get; private set; }

        public static StorageCondition Ambient => new StorageCondition(
            minTemperature: 10,
            maxTemperature: 30,
            targetHumidity: 40
        );

        public static StorageCondition Refrigerated => new StorageCondition(
            minTemperature: 1,
            maxTemperature: 4,
            targetHumidity: 85
        );

        public static StorageCondition Frozen => new StorageCondition(
            minTemperature: -25,
            maxTemperature: -15,
            targetHumidity: 90
        );

        public static StorageCondition DryStorage => new StorageCondition(
            minTemperature: 10,
            maxTemperature: 25,
            targetHumidity: 30,
            specialRequirements: "Keep dry"
        );

        public bool IsCompatibleWith(StorageCondition other)
        {
            if (MinTemperature.HasValue && other.MaxTemperature.HasValue && MinTemperature > other.MaxTemperature)
                return false;

            if (MaxTemperature.HasValue && other.MinTemperature.HasValue && MaxTemperature < other.MinTemperature)
                return false;

            if (TargetHumidity.HasValue && other.TargetHumidity.HasValue)
            {
                double humidityDifference = Math.Abs(TargetHumidity.Value - other.TargetHumidity.Value);
                if (humidityDifference > 20) // 20% tolerance
                    return false;
            }

            return true;
        }

        public bool IsWithinConditions(double temperature, double humidity)
        {
            if (MinTemperature.HasValue && temperature < MinTemperature.Value)
                return false;

            if (MaxTemperature.HasValue && temperature > MaxTemperature.Value)
                return false;

            if (TargetHumidity.HasValue)
            {
                double humidityDifference = Math.Abs(humidity - TargetHumidity.Value);
                if (humidityDifference > 10) // 10% tolerance
                    return false;
            }

            return true;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            StorageCondition other = (StorageCondition)obj;
            return MinTemperature == other.MinTemperature &&
                   MaxTemperature == other.MaxTemperature &&
                   TargetHumidity == other.TargetHumidity &&
                   SpecialRequirements == other.SpecialRequirements;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(MinTemperature, MaxTemperature, TargetHumidity, SpecialRequirements);
        }

        public override string ToString()
        {
            string temperatureRange = string.Empty;
            if (MinTemperature.HasValue && MaxTemperature.HasValue)
                temperatureRange = $"{MinTemperature}°C to {MaxTemperature}°C";
            else if (MinTemperature.HasValue)
                temperatureRange = $"Above {MinTemperature}°C";
            else if (MaxTemperature.HasValue)
                temperatureRange = $"Below {MaxTemperature}°C";

            string humidity = TargetHumidity.HasValue ? $", {TargetHumidity}% humidity" : "";
            string requirements = !string.IsNullOrWhiteSpace(SpecialRequirements) ? $", {SpecialRequirements}" : "";

            if (string.IsNullOrWhiteSpace(temperatureRange) && string.IsNullOrWhiteSpace(humidity) && string.IsNullOrWhiteSpace(requirements))
                return "No specific conditions";

            return $"{temperatureRange}{humidity}{requirements}";
        }
    }
}
