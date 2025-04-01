# Restaurant Management - Inventory Service Application Layer

## Overview

The Application Layer is the heart of the Restaurant Management Inventory Service, serving as an intermediary between the presentation layer (API) and the domain layer. It implements the CQRS (Command Query Responsibility Segregation) pattern using MediatR to handle commands and queries separately, promoting a clean separation of concerns.

This layer contains all the application logic, coordinating the flow of data between the outer and inner layers, and orchestrating domain objects to solve user problems without containing business rules directly.

## Architecture

The Application Layer follows clean architecture principles:

- **Commands**: Define operations that change state
- **Queries**: Define operations that read state
- **Behaviors**: Cross-cutting concerns applied to commands/queries
- **Services**: High-level application services
- **DTOs**: Data Transfer Objects for communication with other layers
- **Validators**: Validation rules for commands and queries
- **Mappings**: Mapping configurations between entities and DTOs
- **Event Handlers**: Handle domain events

## Directory Structure

```plaintext
RestaurantManagement.InventoryService.Application/
├── Behaviors/
│   ├── LoggingBehavior.cs
│   ├── PerformanceBehavior.cs
│   └── ValidationBehavior.cs
├── Commands/
│   ├── InventoryItems/
│   │   ├── CreateInventoryItemCommand.cs
│   │   ├── UpdateInventoryItemCommand.cs
│   │   └── DeleteInventoryItemCommand.cs
│   ├── Batches/
│   │   ├── CreateBatchCommand.cs
│   │   └── UpdateBatchCommand.cs
│   ├── CountSheets/
│   │   ├── CreateCountSheetCommand.cs
│   │   └── CompleteCountSheetCommand.cs
│   ├── Transactions/
│   │   ├── CreateInventoryTransactionCommand.cs
│   │   ├── CompleteInventoryTransactionCommand.cs
│   │   └── CancelInventoryTransactionCommand.cs
│   ├── Locations/
│   │   ├── CreateLocationCommand.cs
│   │   └── UpdateLocationCommand.cs
│   └── Vendors/
│       ├── CreateVendorCommand.cs
│       └── UpdateVendorCommand.cs
├── EventHandlers/
│   ├── StockLevelBelowThresholdEventHandler.cs
│   ├── BatchExpiringEventHandler.cs
│   └── InventoryTransactionCompletedEventHandler.cs
├── Exceptions/
│   ├── ValidationException.cs
│   ├── NotFoundException.cs
│   ├── ForbiddenAccessException.cs
│   └── BusinessRuleViolationException.cs
├── Interfaces/
│   ├── ICurrentUserService.cs
│   ├── IDateTimeService.cs
│   ├── IEmailService.cs
│   ├── INotificationService.cs
│   ├── IFileStorageService.cs
│   └── IBarcodeService.cs
├── Mappings/
│   └── InventoryMappingProfile.cs
├── Models/
│   ├── EmailMessage.cs
│   ├── Notification.cs
│   ├── NotificationType.cs
│   ├── EmailSettings.cs
│   └── FileStorageSettings.cs
├── Queries/
│   ├── InventoryItems/
│   │   ├── GetInventoryItemByIdQuery.cs
│   │   ├── GetInventoryItemBySKUQuery.cs
│   │   ├── GetInventoryItemsQuery.cs
│   │   ├── GetInventoryItemsForReorderQuery.cs
│   │   └── GetInventoryItemsWithStockAtLocationQuery.cs
│   ├── Batches/
│   │   ├── GetBatchByIdQuery.cs
│   │   ├── GetBatchesByItemQuery.cs
│   │   └── GetExpiringBatchesQuery.cs
│   ├── CountSheets/
│   │   ├── GetCountSheetByIdQuery.cs
│   │   └── GetCountSheetsByDateRangeQuery.cs
│   ├── Transactions/
│   │   ├── GetInventoryTransactionByIdQuery.cs
│   │   ├── GetPendingInventoryTransactionsQuery.cs
│   │   └── GetInventoryTransactionsByDateRangeQuery.cs
│   ├── Locations/
│   │   ├── GetLocationByIdQuery.cs
│   │   └── GetAllLocationsQuery.cs
│   └── Vendors/
│       ├── GetVendorByIdQuery.cs
│       └── GetAllVendorsQuery.cs
├── Services/
│   ├── InventoryService.cs
│   ├── TransactionService.cs
│   └── ReportingService.cs
├── Validators/
│   ├── CreateInventoryTransactionValidator.cs
│   ├── CreateCountSheetValidator.cs
│   ├── CompleteCountSheetValidator.cs
│   ├── CreateBatchValidator.cs
│   ├── CreateVendorValidator.cs
│   ├── CreateLocationValidator.cs
│   └── CreateReservationValidator.cs
├── DependencyInjection.cs
└── RestaurantManagement.InventoryService.Application.csproj
```

## Key Components

### Commands & Queries

#### Commands

Commands are used to perform actions that change the state of the system. Each command has:

1. **Command class**: Defines the request data (input)
2. **Validator**: Validates the command data using FluentValidation
3. **Handler**: Processes the command and returns a result

Example command structure:

```csharp
// Command definition
public class CreateInventoryItemCommand : IRequest<string>
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string SKU { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string UnitOfMeasure { get; set; } = string.Empty;
    public double ReorderThreshold { get; set; }
    public double MinimumOrderQuantity { get; set; }
    public double DefaultOrderQuantity { get; set; }
}

// Command validator
public class CreateInventoryItemCommandValidator : AbstractValidator<CreateInventoryItemCommand>
{
    public CreateInventoryItemCommandValidator()
    {
        RuleFor(v => v.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(100).WithMessage("Name must not exceed 100 characters.");
        // Additional validation rules...
    }
}

// Command handler
public class CreateInventoryItemCommandHandler : IRequestHandler<CreateInventoryItemCommand, string>
{
    // Dependencies...
    
    public async Task<string> Handle(CreateInventoryItemCommand request, CancellationToken cancellationToken)
    {
        // Implementation...
    }
}
```

#### Queries

Queries are used to retrieve data without modifying the system state. Each query has:

1. **Query class**: Defines the request parameters
2. **Handler**: Retrieves and returns the requested data
3. **DTO (Data Transfer Object)**: Represents the response data

Example query structure:

```csharp
// Query definition
public class GetInventoryItemByIdQuery : IRequest<InventoryItemDto>
{
    public string Id { get; set; } = string.Empty;
}

// Query handler
public class GetInventoryItemByIdQueryHandler : IRequestHandler<GetInventoryItemByIdQuery, InventoryItemDto>
{
    // Dependencies...
    
    public async Task<InventoryItemDto> Handle(GetInventoryItemByIdQuery request, CancellationToken cancellationToken)
    {
        // Implementation...
    }
}
```

### Behaviors

Behaviors provide cross-cutting concerns to commands and queries through the MediatR pipeline:

1. **ValidationBehavior**: Validates commands using FluentValidation before they reach the handler
2. **LoggingBehavior**: Logs request and response details
3. **PerformanceBehavior**: Tracks and logs performance metrics

### Event Handlers

Event handlers process domain events, responding to significant changes in the system:

- **StockLevelBelowThresholdEventHandler**: Handles notifications for low stock
- **BatchExpiringEventHandler**: Handles alerts for batches nearing expiration
- **InventoryTransactionCompletedEventHandler**: Updates stock levels when transactions complete

### Services

Application services provide a higher-level API for the presentation layer, orchestrating commands and queries:

- **InventoryService**: Manages inventory items and stock levels
- **TransactionService**: Handles inventory transactions (receipts, consumption, transfers)
- **ReportingService**: Generates reports and analytics

### DTOs (Data Transfer Objects)

DTOs are used to transfer data between layers without exposing domain entities:

- **InventoryItemDto**: Represents an inventory item for read operations
- **InventoryItemListDto**: Represents a list item for inventory items
- **InventoryItemWithStockLevelDto**: Combines inventory item with stock level information

### Validators

Validators ensure that commands and queries contain valid data:

- **CreateInventoryTransactionValidator**
- **CreateCountSheetValidator**
- **CreateBatchValidator**
- **CreateVendorValidator**
- **CreateLocationValidator**

### Mappings

AutoMapper profiles define mappings between domain entities and DTOs:

- **InventoryMappingProfile**: Maps entities to DTOs and vice versa

## Technical Stack

- **.NET 9.0**: Framework for building the application
- **MediatR**: Implements the mediator pattern for CQRS
- **FluentValidation**: Validation library
- **AutoMapper**: Object-object mapping library

## Dependencies

The Application Layer depends on:

- **Domain Layer**: Core business logic and entities
- **Application.Contracts**: Public contracts (commands, queries, events)

## Configuration

The Application Layer is configured through the `DependencyInjection.cs` class, which registers all the necessary services with the dependency injection container:

```csharp
public static IServiceCollection AddApplication(this IServiceCollection services)
{
    // Add MediatR
    services.AddMediatR(cfg =>
    {
        cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
    });

    // Add AutoMapper
    services.AddAutoMapper(Assembly.GetExecutingAssembly());

    // Add FluentValidation
    services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

    // Add MediatR behaviors
    services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
    services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
    services.AddTransient(typeof(IPipelineBehavior<,>), typeof(PerformanceBehavior<,>));

    // Add application services
    services.AddScoped<IInventoryService, InventoryService>();
    services.AddScoped<ITransactionService, TransactionService>();
    services.AddScoped<IReportingService, ReportingService>();

    return services;
}
```

## Usage

### Using Commands

To execute a command:

```csharp
// Create a command instance
var command = new CreateInventoryItemCommand
{
    Name = "Tomatoes",
    Description = "Fresh Roma tomatoes",
    SKU = "PROD-001",
    Category = "Produce",
    UnitOfMeasure = "kg",
    ReorderThreshold = 5.0,
    MinimumOrderQuantity = 10.0,
    DefaultOrderQuantity = 20.0
};

// Send the command via MediatR
var result = await _mediator.Send(command);
```

### Using Queries

To execute a query:

```csharp
// Create a query instance
var query = new GetInventoryItemByIdQuery { Id = "some-inventory-item-id" };

// Send the query via MediatR
var inventoryItem = await _mediator.Send(query);
```

### Using Application Services

Application services provide a higher-level API for the presentation layer:

```csharp
// Inject the service
private readonly IInventoryService _inventoryService;

// Use the service
var inventoryItem = await _inventoryService.GetInventoryItemByIdAsync("some-inventory-item-id");
```

## Exception Handling

The Application Layer defines several exception types:

- **ValidationException**: Thrown when command validation fails
- **NotFoundException**: Thrown when an entity cannot be found
- **ForbiddenAccessException**: Thrown when access to a resource is forbidden
- **BusinessRuleViolationException**: Thrown when a business rule is violated

These exceptions are handled by the API layer to return appropriate HTTP status codes.

## Best Practices

1. **Keep Handlers Focused**: Each handler should do one thing well
2. **Validate Input**: All commands should have validators
3. **Use DTOs**: Never expose domain entities directly
4. **Follow CQRS**: Keep commands and queries separate
5. **Use Domain Events**: For side effects and integration scenarios
6. **Log Appropriately**: Include relevant context in logs
7. **Unit Test**: Test handlers, validators, and services

## Testing

Commands, queries, and services can be tested using these approaches:

- **Unit Tests**: Test individual handlers and validators
- **Integration Tests**: Test services with in-memory repositories

```csharp
[Fact]
public async Task CreateInventoryItem_WithValidCommand_ShouldCreateItem()
{
    // Arrange
    var command = new CreateInventoryItemCommand
    {
        Name = "Test Item",
        SKU = "TEST-001",
        // Other properties...
    };
    
    // Act
    var result = await _handler.Handle(command, CancellationToken.None);
    
    // Assert
    Assert.NotNull(result);
    // Additional assertions...
}
```

## Contributing

When adding new functionality to the Application Layer:

1. Define commands/queries in their respective directories
2. Add validators for new commands
3. Implement handlers with proper error handling
4. Add DTOs for new entities
5. Update AutoMapper profiles
6. Add unit tests
7. Update application services as needed
