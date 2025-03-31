using System;
using System.Collections.Generic;

namespace RestaurantManagement.InventoryService.Domain.Exceptions
{
    /// <summary>
    /// Exception thrown when an entity fails validation.
    /// </summary>
    public class ValidationException : DomainException
    {
        /// <summary>
        /// Gets the validation errors.
        /// </summary>
        public IReadOnlyDictionary<string, string[]> Errors { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationException"/> class.
        /// </summary>
        /// <param name="errors">The validation errors.</param>
        public ValidationException(Dictionary<string, string[]> errors)
            : base("VAL001", "One or more validation errors occurred.")
        {
            Errors = errors;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationException"/> class.
        /// </summary>
        /// <param name="property">The property that failed validation.</param>
        /// <param name="error">The validation error message.</param>
        public ValidationException(string property, string error)
            : base("VAL001", $"Validation error for property '{property}': {error}")
        {
            var errors = new Dictionary<string, string[]> { { property, new[] { error } } };

            Errors = errors;
        }
    }

    /// <summary>
    /// Exception thrown when a required property is null or empty.
    /// </summary>
    public class RequiredPropertyException : ValidationException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RequiredPropertyException"/> class.
        /// </summary>
        /// <param name="property">The required property that was null or empty.</param>
        public RequiredPropertyException(string property)
            : base(property, $"Property '{property}' is required and cannot be null or empty.") { }
    }

    /// <summary>
    /// Exception thrown when a property exceeds its maximum length.
    /// </summary>
    public class MaxLengthExceededException : ValidationException
    {
        /// <summary>
        /// Gets the maximum allowed length.
        /// </summary>
        public int MaxLength { get; }

        /// <summary>
        /// Gets the actual length.
        /// </summary>
        public int ActualLength { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MaxLengthExceededException"/> class.
        /// </summary>
        /// <param name="property">The property that exceeded its maximum length.</param>
        /// <param name="maxLength">The maximum allowed length.</param>
        /// <param name="actualLength">The actual length.</param>
        public MaxLengthExceededException(string property, int maxLength, int actualLength)
            : base(
                property,
                $"Property '{property}' has a maximum length of {maxLength} but was {actualLength} characters long."
            )
        {
            MaxLength = maxLength;
            ActualLength = actualLength;
        }
    }

    /// <summary>
    /// Exception thrown when a numeric property is outside the valid range.
    /// </summary>
    public class NumericRangeException : ValidationException
    {
        /// <summary>
        /// Gets the minimum allowed value, if specified.
        /// </summary>
        public double? MinValue { get; }

        /// <summary>
        /// Gets the maximum allowed value, if specified.
        /// </summary>
        public double? MaxValue { get; }

        /// <summary>
        /// Gets the actual value.
        /// </summary>
        public double ActualValue { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="NumericRangeException"/> class for a value below the minimum.
        /// </summary>
        /// <param name="property">The property that was below the minimum.</param>
        /// <param name="minValue">The minimum allowed value.</param>
        /// <param name="actualValue">The actual value.</param>
        public NumericRangeException(string property, double minValue, double actualValue)
            : base(
                property,
                $"Property '{property}' must be at least {minValue} but was {actualValue}."
            )
        {
            MinValue = minValue;
            ActualValue = actualValue;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NumericRangeException"/> class for a value outside the range.
        /// </summary>
        /// <param name="property">The property that was outside the range.</param>
        /// <param name="minValue">The minimum allowed value.</param>
        /// <param name="maxValue">The maximum allowed value.</param>
        /// <param name="actualValue">The actual value.</param>
        public NumericRangeException(
            string property,
            double minValue,
            double maxValue,
            double actualValue
        )
            : base(
                property,
                $"Property '{property}' must be between {minValue} and {maxValue} but was {actualValue}."
            )
        {
            MinValue = minValue;
            MaxValue = maxValue;
            ActualValue = actualValue;
        }
    }

    /// <summary>
    /// Exception thrown when a date property is outside the valid range.
    /// </summary>
    public class DateRangeException : ValidationException
    {
        /// <summary>
        /// Gets the minimum allowed date, if specified.
        /// </summary>
        public DateTime? MinDate { get; }

        /// <summary>
        /// Gets the maximum allowed date, if specified.
        /// </summary>
        public DateTime? MaxDate { get; }

        /// <summary>
        /// Gets the actual date.
        /// </summary>
        public DateTime ActualDate { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DateRangeException"/> class for a date before the minimum.
        /// </summary>
        /// <param name="property">The property that was before the minimum.</param>
        /// <param name="minDate">The minimum allowed date.</param>
        /// <param name="actualDate">The actual date.</param>
        public DateRangeException(string property, DateTime minDate, DateTime actualDate)
            : base(
                property,
                $"Property '{property}' must be on or after {minDate:d} but was {actualDate:d}."
            )
        {
            MinDate = minDate;
            ActualDate = actualDate;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DateRangeException"/> class for a date outside the range.
        /// </summary>
        /// <param name="property">The property that was outside the range.</param>
        /// <param name="minDate">The minimum allowed date.</param>
        /// <param name="maxDate">The maximum allowed date.</param>
        /// <param name="actualDate">The actual date.</param>
        public DateRangeException(
            string property,
            DateTime minDate,
            DateTime maxDate,
            DateTime actualDate
        )
            : base(
                property,
                $"Property '{property}' must be between {minDate:d} and {maxDate:d} but was {actualDate:d}."
            )
        {
            MinDate = minDate;
            MaxDate = maxDate;
            ActualDate = actualDate;
        }
    }

    /// <summary>
    /// Exception thrown when a property has an invalid format.
    /// </summary>
    public class InvalidFormatException : ValidationException
    {
        /// <summary>
        /// Gets the expected format.
        /// </summary>
        public string ExpectedFormat { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidFormatException"/> class.
        /// </summary>
        /// <param name="property">The property with an invalid format.</param>
        /// <param name="expectedFormat">The expected format.</param>
        public InvalidFormatException(string property, string expectedFormat)
            : base(
                property,
                $"Property '{property}' has an invalid format. Expected format: {expectedFormat}"
            )
        {
            ExpectedFormat = expectedFormat;
        }
    }
}
