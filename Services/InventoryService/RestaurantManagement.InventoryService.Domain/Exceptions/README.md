# Domain Exceptions

This directory contains custom exception classes for the Inventory Management System. These exceptions help communicate domain-specific errors in a structured and consistent way, making error handling more robust throughout the application.

## Core Components

| File | Description |
|------|-------------|
| `DomainException.cs` | Base exception class for all domain-specific exceptions |
| `ValidationExceptions.cs` | Generic validation exceptions for entity validation |

## Entity-Specific Exceptions

Each major entity in the domain has its own set of exceptions:

| File | Description |
|------|-------------|
| `InventoryItemExceptions.cs` | Exceptions related to inventory items (duplicate SKUs, invalid thresholds) |
| `StockLevelExceptions.cs` | Exceptions related to stock levels (negative quantities, insufficient stock) |
| `LocationExceptions.cs` | Exceptions related to storage locations (duplicate names, temperature constraints) |
| `TransactionExceptions.cs` | Exceptions related to inventory transactions (invalid types, quantities) |
| `BatchExceptions.cs` | Exceptions related to batch tracking (expiration dates, quantities) |
| `VendorExceptions.cs` | Exceptions related to vendors (duplicate names, inactive vendors) |
| `CountSheetExceptions.cs` | Exceptions related to physical inventory counting process |

## Exception Structure

All domain exceptions:

- Inherit from `DomainException`, which inherits from `System.Exception`
- Include an `ErrorCode` property for easy identification and categorization
- Include relevant entity IDs and other contextual information as properties
- Provide descriptive error messages that explain the issue clearly

## Error Code Conventions

Error codes follow this format: `XXX###`

- `XXX` is a three-letter prefix that indicates the entity/category:
  - `INV`: Inventory Item
  - `STK`: Stock Level
  - `LOC`: Location
  - `TRX`: Transaction
  - `BAT`: Batch
  - `VEN`: Vendor
  - `CNT`: Count Sheet
  - `VAL`: Validation
- `###` is a three-digit number (starting from 001) that uniquely identifies the error within the category

## Usage Pattern

Exceptions should be thrown when:

1. An operation would violate domain rules or invariants
2. Required entities don't exist or are in an invalid state
3. Input validation fails

Example usage:

```csharp
public void UpdateTemperatureRange(double? minTemperature, double? maxTemperature, string modifiedBy)
{
    if (minTemperature.HasValue && maxTemperature.HasValue && minTemperature.Value > maxTemperature.Value)
    {
        throw new InvalidTemperatureRangeException(minTemperature.Value, maxTemperature.Value);
    }

    // Update temperature range
    MinTemperature = minTemperature;
    MaxTemperature = maxTemperature;
    LastModifiedAt = DateTime.UtcNow;
    LastModifiedBy = modifiedBy;
}
```

## Exception Handling

These exceptions should be handled at appropriate levels in the application:

- **Domain Layer**: Throws domain exceptions when rules are violated
- **Application Layer**: Catches domain exceptions, logs details, and translates to appropriate responses
- **Presentation Layer**: Displays user-friendly error messages based on exception type

This approach ensures that domain rules are strictly enforced while providing clear feedback to users when operations cannot be completed.
