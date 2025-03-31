using System;

namespace RestaurantManagement.InventoryService.Domain.Exceptions
{
    /// <summary>
    /// Exception thrown when attempting to create a location with a duplicate name.
    /// </summary>
    public class DuplicateLocationNameException : DomainException
    {
        /// <summary>
        /// Gets the location name that caused the duplicate.
        /// </summary>
        public string LocationName { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DuplicateLocationNameException"/> class.
        /// </summary>
        /// <param name="locationName">The location name that caused the duplicate.</param>
        public DuplicateLocationNameException(string locationName)
            : base("LOC001", $"A location with the name '{locationName}' already exists.")
        {
            LocationName = locationName;
        }
    }

    /// <summary>
    /// Exception thrown when attempting to set invalid temperature ranges.
    /// </summary>
    public class InvalidTemperatureRangeException : DomainException
    {
        /// <summary>
        /// Gets the minimum temperature.
        /// </summary>
        public double? MinTemperature { get; }

        /// <summary>
        /// Gets the maximum temperature.
        /// </summary>
        public double? MaxTemperature { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidTemperatureRangeException"/> class.
        /// </summary>
        /// <param name="minTemperature">The minimum temperature.</param>
        /// <param name="maxTemperature">The maximum temperature.</param>
        public InvalidTemperatureRangeException(double? minTemperature, double? maxTemperature)
            : base(
                "LOC002",
                $"The minimum temperature ({minTemperature}) cannot be greater than the maximum temperature ({maxTemperature})."
            )
        {
            MinTemperature = minTemperature;
            MaxTemperature = maxTemperature;
        }
    }

    /// <summary>
    /// Exception thrown when attempting to store an item in a location with incompatible storage requirements.
    /// </summary>
    public class IncompatibleStorageRequirementsException : DomainException
    {
        /// <summary>
        /// Gets the location ID.
        /// </summary>
        public string LocationId { get; }

        /// <summary>
        /// Gets the location name.
        /// </summary>
        public string LocationName { get; }

        /// <summary>
        /// Gets the inventory item ID.
        /// </summary>
        public string ItemId { get; }

        /// <summary>
        /// Gets the inventory item name.
        /// </summary>
        public string ItemName { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="IncompatibleStorageRequirementsException"/> class.
        /// </summary>
        /// <param name="locationId">The location ID.</param>
        /// <param name="locationName">The location name.</param>
        /// <param name="itemId">The inventory item ID.</param>
        /// <param name="itemName">The inventory item name.</param>
        /// <param name="reason">The detailed reason for the incompatibility.</param>
        public IncompatibleStorageRequirementsException(
            string locationId,
            string locationName,
            string itemId,
            string itemName,
            string reason
        )
            : base(
                "LOC003",
                $"Item '{itemName}' cannot be stored in location '{locationName}'. {reason}"
            )
        {
            LocationId = locationId;
            LocationName = locationName;
            ItemId = itemId;
            ItemName = itemName;
        }
    }

    /// <summary>
    /// Exception thrown when a location is not found.
    /// </summary>
    public class LocationNotFoundException : DomainException
    {
        /// <summary>
        /// Gets the location identifier that was not found.
        /// </summary>
        public string LocationIdentifier { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="LocationNotFoundException"/> class.
        /// </summary>
        /// <param name="locationId">The location ID that was not found.</param>
        public LocationNotFoundException(string locationId)
            : base("LOC004", $"Location with ID '{locationId}' was not found.")
        {
            LocationIdentifier = locationId;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LocationNotFoundException"/> class with a location name.
        /// </summary>
        /// <param name="locationName">The location name that was not found.</param>
        /// <param name="isName">Flag indicating that the identifier is a name.</param>
        public LocationNotFoundException(string locationName, bool isName)
            : base("LOC004", $"Location with name '{locationName}' was not found.")
        {
            LocationIdentifier = locationName;
        }
    }

    /// <summary>
    /// Exception thrown when attempting to use an inactive location.
    /// </summary>
    public class InactiveLocationException : DomainException
    {
        /// <summary>
        /// Gets the location ID.
        /// </summary>
        public string LocationId { get; }

        /// <summary>
        /// Gets the location name.
        /// </summary>
        public string LocationName { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="InactiveLocationException"/> class.
        /// </summary>
        /// <param name="locationId">The location ID.</param>
        /// <param name="locationName">The location name.</param>
        public InactiveLocationException(string locationId, string locationName)
            : base("LOC005", $"Location '{locationName}' is inactive and cannot be used.")
        {
            LocationId = locationId;
            LocationName = locationName;
        }
    }
}
