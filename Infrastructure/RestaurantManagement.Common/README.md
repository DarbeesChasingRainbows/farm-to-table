# RestaurantManagement.Common

A shared library of common components, models, and utilities for the Restaurant Management System microservices architecture.

## Overview

The `RestaurantManagement.Common` library provides cross-cutting concerns and shared components that ensure consistency across all microservices in the Restaurant Management ecosystem. This library reduces code duplication and enforces standardized patterns across the entire system.

## Table of Contents

- [Project Structure](#project-structure)
- [Key Components](#key-components)
  - [Models](#models)
  - [Behaviors](#behaviors)
  - [Exceptions](#exceptions)
  - [Extensions](#extensions)
  - [Helpers](#helpers)
- [Installation](#installation)
- [Usage](#usage)
- [Contributing](#contributing)

## Project Structure

```plaintext
RestaurantManagement.Common/
├── Behaviors/              # MediatR behaviors for cross-cutting concerns
├── Constants/              # Common constant values
├── Exceptions/             # Standardized exception types
├── Extensions/             # Extension methods
├── Helpers/                # Utility helper classes
├── Models/                 # Shared data models
│   ├── Dto/                # Data transfer objects
│   └── Enums/              # Shared enumerations
└── Validators/             # Common validation logic
```

## Key Components

### Models

#### PaginatedResult

A standardized model for paginated API responses across all services.

```csharp
var paginatedResult = PaginatedResult<Product>.Create(productsQuery, pageNumber, pageSize);
```

Usage in API controllers:

```csharp
[HttpGet]
public async Task<ActionResult<PaginatedResult<ProductDto>>> GetProducts([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
{
    var result = await _mediator.Send(new GetProductsQuery { Page = page, PageSize = pageSize });
    return Ok(result);
}
```

#### ApiResponse

A standardized wrapper for API responses providing consistent structure across all services.

```csharp
// Success response
return ApiResponse<OrderDto>.Succeed(orderDto, "Order created successfully");

// Error response
return ApiResponse<OrderDto>.Fail("Order creation failed", new[] { "Insufficient inventory" });
```

### Behaviors

#### ValidationBehavior

Automatic request validation for MediatR pipeline using FluentValidation.

Registration:

```csharp
services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
```

Usage with commands:

```csharp
public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Price).GreaterThan(0);
    }
}
```

#### LoggingBehavior

Provides automatic logging of all requests and responses in the MediatR pipeline.

#### PerformanceBehavior

Monitors and logs slow-running requests to help identify performance bottlenecks.

### Exceptions

#### DomainException

Base exception type for all domain-specific exceptions.

#### NotFoundException

Thrown when an entity cannot be found.

```csharp
throw new NotFoundException(nameof(Product), productId);
```

#### ValidationException

Thrown when request validation fails.

#### BusinessRuleViolationException

Thrown when a business rule is violated.

```csharp
if (order.Status == OrderStatus.Completed)
    throw new BusinessRuleViolationException("Cannot cancel a completed order");
```

#### ForbiddenAccessException

Thrown when access to a resource is forbidden.

### Extensions

#### EnumExtensions

Utility extensions for working with enumerations.

```csharp
// Get description attribute value from enum
OrderStatus.Pending.GetDescription(); // Returns "Order is pending"
```

#### StringExtensions

Common string manipulation utilities.

```csharp
// Convert to kebab-case
"ProductCategory".ToKebabCase(); // Returns "product-category"
```

#### QueryableExtensions

Extensions for IQueryable to simplify common data access patterns.

```csharp
// Get paginated results
var pagedProducts = await productsQuery.ToPaginatedListAsync(pageNumber, pageSize);
```

### Helpers

#### DateTimeProvider

Provides consistent datetime handling across services.

```csharp
var now = DateTimeProvider.UtcNow;
```

#### Guard

Simplifies input validation and defensive programming.

```csharp
Guard.Against.Null(product, nameof(product));
Guard.Against.NegativeOrZero(quantity, nameof(quantity));
```

## Installation

Add a reference to the `RestaurantManagement.Common` project in your microservice:

```xml
<ItemGroup>
  <ProjectReference Include="..\..\..\Infrastructure\RestaurantManagement.Common\RestaurantManagement.Common.csproj" />
</ItemGroup>
```

## Usage

### Adding to a Microservice

1. Add a project reference as shown in the Installation section.

2. Register common components in your Startup.cs or Program.cs:

```csharp
// Register MediatR behaviors
services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
services.AddTransient(typeof(IPipelineBehavior<,>), typeof(PerformanceBehavior<,>));

// Register FluentValidation
services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
```

### Examples

#### API Controller with Standardized Responses

```csharp
[ApiController]
[Route("api/v1/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProductsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<PaginatedResult<ProductDto>>>> GetProducts(
        [FromQuery] int page = 1, 
        [FromQuery] int pageSize = 10)
    {
        var result = await _mediator.Send(new GetProductsQuery { Page = page, PageSize = pageSize });
        return Ok(ApiResponse<PaginatedResult<ProductDto>>.Succeed(result));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<ProductDto>>> GetProduct(Guid id)
    {
        try 
        {
            var result = await _mediator.Send(new GetProductByIdQuery { Id = id });
            return Ok(ApiResponse<ProductDto>.Succeed(result));
        }
        catch (NotFoundException ex)
        {
            return NotFound(ApiResponse<ProductDto>.Fail(ex.Message));
        }
    }
}
```

#### Command Handler with Domain Validation

```csharp
public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Guid>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateProductCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        // Check for duplicate SKU
        var existingProduct = await _unitOfWork.Products.FindBySKUAsync(request.SKU);
        if (existingProduct != null)
        {
            throw new BusinessRuleViolationException($"A product with SKU '{request.SKU}' already exists");
        }

        var product = new Product
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Description = request.Description,
            SKU = request.SKU,
            Price = request.Price,
            Category = request.Category
        };

        await _unitOfWork.Products.AddAsync(product);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return product.Id;
    }
}
```

## Contributing

When adding new components to the Common library, follow these guidelines:

1. **Generality**: Only add components that are truly shared across multiple services
2. **Immutability**: Prefer immutable objects for shared models
3. **Minimal Dependencies**: Keep external dependencies to a minimum
4. **Documentation**: Add XML documentation to all public types and members
5. **Unit Tests**: Ensure comprehensive test coverage for all components

### Adding a New Component

1. Create the component in the appropriate directory
2. Add unit tests
3. Update this README if appropriate
4. Submit a pull request
