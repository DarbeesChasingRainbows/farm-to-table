# RestaurantManagement.ServiceDefaults

Global infrastructure components providing standardized configuration and capabilities for all microservices in the Restaurant Management System.

## Overview

The `RestaurantManagement.ServiceDefaults` library serves as the foundation for all microservices within the Restaurant Management ecosystem. It establishes consistent infrastructure components, configuration patterns, and cross-cutting concerns across the distributed application architecture.

By centralizing these defaults, we ensure all services behave consistently with regard to resilience, observability, health monitoring, and other critical infrastructure concerns.

## Table of Contents

- [RestaurantManagement.ServiceDefaults](#restaurantmanagementservicedefaults)
  - [Overview](#overview)
  - [Table of Contents](#table-of-contents)
  - [Features](#features)
  - [Project Structure](#project-structure)
  - [Getting Started](#getting-started)
    - [Prerequisites](#prerequisites)
    - [Installation](#installation)
    - [Basic Setup](#basic-setup)
  - [Key Components](#key-components)
    - [Service Discovery](#service-discovery)
    - [Resilience Patterns](#resilience-patterns)
    - [Health Checks](#health-checks)
    - [Observability](#observability)
    - [Global Exception Handling](#global-exception-handling)
    - [Authentication and Authorization](#authentication-and-authorization)
    - [API Documentation](#api-documentation)
  - [Configuration](#configuration)
    - [appsettings.json Example](#appsettingsjson-example)
  - [Usage](#usage)
    - [Standard API Controller](#standard-api-controller)
    - [Consuming Services with Resilience](#consuming-services-with-resilience)
    - [Health Check Registration](#health-check-registration)
  - [Contributing](#contributing)
    - [Adding a New Feature](#adding-a-new-feature)
    - [Required Skills](#required-skills)

## Features

- **Service Discovery**: Automatic registration and discovery of microservices
- **Resilience Patterns**: Circuit breakers, retries, and timeouts for HTTP clients
- **Health Monitoring**: Standardized health checks and endpoints
- **Observability**: Unified OpenTelemetry instrumentation for metrics, tracing, and logging
- **Global Exception Handling**: Consistent error handling across all services
- **Authentication**: JWT-based authentication integration
- **API Documentation**: Swagger/OpenAPI with versioning support
- **Environment-specific Configuration**: Development vs. production settings

## Project Structure

```plaintext
RestaurantManagement.ServiceDefaults/
├── Authentication/           # Authentication and authorization components
├── Middleware/               # Middleware components for request pipeline
├── Observability/            # Telemetry and logging configuration
├── Resilience/               # Circuit breakers, retries, and timeout policies
├── ServiceDiscovery/         # Service registration and discovery
├── Swagger/                  # OpenAPI documentation setup
├── Extensions.cs             # Main extension methods for service defaults
└── DependencyInjection.cs    # Registration of all components
```

## Getting Started

### Prerequisites

- .NET 8.0 SDK or higher
- ASP.NET Core understanding
- Basic knowledge of microservices architecture

### Installation

Add a reference to the `RestaurantManagement.ServiceDefaults` project in your microservice:

```xml
<ItemGroup>
  <ProjectReference Include="..\..\..\Infrastructure\RestaurantManagement.ServiceDefaults\RestaurantManagement.ServiceDefaults.csproj" />
</ItemGroup>
```

### Basic Setup

In your microservice's `Program.cs`:

```csharp
var builder = WebApplication.CreateBuilder(args);

// Add service defaults (health checks, OpenTelemetry, service discovery, etc.)
builder.AddServiceDefaults();

// Add service-specific components
// ...

var app = builder.Build();

// Map default endpoints (health, readiness, etc.)
app.MapDefaultEndpoints();

// ...

app.Run();
```

## Key Components

### Service Discovery

Service discovery enables microservices to locate and communicate with each other without hardcoded endpoints.

```csharp
// Configuring service discovery
public static IServiceCollection AddRestaurantServiceDiscovery(this IServiceCollection services, IConfiguration configuration)
{
    // Add base service discovery
    services.AddServiceDiscovery();
    
    // Configure service discovery options
    services.Configure<ServiceDiscoveryOptions>(options =>
    {
        // Force HTTPS for service-to-service communication in production
        if (!string.IsNullOrEmpty(configuration["Environment"]) && 
            configuration["Environment"].Equals("Production", StringComparison.OrdinalIgnoreCase))
        {
            options.AllowedSchemes = ["https"];
        }
    });
    
    return services;
}
```

When configuring an HTTP client to use service discovery:

```csharp
builder.Services.AddHttpClient<IOrderService, OrderService>()
    .AddServiceDiscovery();
```

### Resilience Patterns

Resilience patterns protect services from cascading failures and ensure system stability.

```csharp
// Adding resilience to an HTTP client
builder.Services.AddHttpClient<IInventoryService, InventoryService>()
    .AddRestaurantResilienceHandler();
```

The resilience handler configures:

- **Retries**: Automatic retry for transient failures
- **Circuit Breaker**: Prevents repeated calls to failing services
- **Timeout**: Ensures requests don't hang indefinitely

### Health Checks

Health checks allow monitoring systems to verify service health and availability.

```csharp
// Adding health checks in a service
builder.AddRestaurantHealthChecks(builder.Configuration);
```

This adds:

- `/health` - Overall service health
- `/health/live` - Liveness checks (is the service running?)
- `/health/ready` - Readiness checks (is the service ready to accept requests?)

Specific health checks are configured based on service dependencies:

- SQL Server connection
- RabbitMQ connection
- Redis connection
- External API dependencies

### Observability

The observability stack provides unified monitoring and debugging capabilities across services.

```csharp
// Configuring observability
builder.ConfigureRestaurantObservability();
```

This sets up:

- **Distributed Tracing**: Track requests across service boundaries
- **Metrics Collection**: Performance and business metrics
- **Structured Logging**: Consistent log format across services
- **Exception Tracking**: Detailed error information

Configuration example in `appsettings.json`:

```json
{
  "ServiceName": "RestaurantManagement.InventoryService",
  "ServiceVersion": "1.0.0",
  "OTEL_EXPORTER_OTLP_ENDPOINT": "http://otel-collector:4317"
}
```

### Global Exception Handling

The global exception handler ensures consistent error responses across all services.

```csharp
// Register in Program.cs
app.UseGlobalExceptionHandling();
```

This middleware:

- Catches all unhandled exceptions
- Maps exception types to appropriate HTTP status codes
- Ensures consistent error response format
- Adds detailed information in development mode

Example error response:

```json
{
  "status": 404,
  "title": "Resource Not Found",
  "detail": "Entity \"Product\" (42) was not found."
}
```

### Authentication and Authorization

Standardized authentication using JWT tokens.

```csharp
// Configure in Program.cs
builder.Services.AddRestaurantAuthentication(builder.Configuration);
```

Configuration in `appsettings.json`:

```json
{
  "Authentication": {
    "Authority": "https://auth.restaurant-management.com",
    "Audience": "inventory-service",
    "DisableHttpsMetadata": false
  }
}
```

Common authorization policies:

```csharp
// Using authorization policies
[Authorize(Policy = "RequireManagerRole")]
[HttpPost]
public async Task<ActionResult> CreateProduct(CreateProductCommand command)
{
    // ...
}
```

### API Documentation

Standardized Swagger/OpenAPI documentation with versioning support.

```csharp
// Configure in Program.cs
builder.Services.AddRestaurantApiDocumentation(builder.Configuration);

// ...

app.UseRestaurantApiDocumentation(app.Services.GetRequiredService<IApiVersionDescriptionProvider>());
```

Configuration in `appsettings.json`:

```json
{
  "Swagger": {
    "ApiName": "Inventory Service API",
    "EnableSwagger": true
  }
}
```

## Configuration

### appsettings.json Example

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "ServiceName": "RestaurantManagement.InventoryService",
  "ServiceVersion": "1.0.0",
  "ConnectionStrings": {
    "DefaultConnection": "Server=.;Database=RestaurantInventory;Trusted_Connection=True;MultipleActiveResultSets=true"
  },
  "RabbitMQ": {
    "Host": "localhost",
    "Username": "guest",
    "Password": "guest"
  },
  "Authentication": {
    "Authority": "https://auth.restaurant-management.com",
    "Audience": "inventory-service"
  },
  "Swagger": {
    "ApiName": "Inventory Service API"
  },
  "OTEL_EXPORTER_OTLP_ENDPOINT": "http://otel-collector:4317"
}
```

## Usage

### Standard API Controller

```csharp
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[Authorize]
public class ProductsController : ControllerBase
{
    // Controller implementation
}
```

### Consuming Services with Resilience

```csharp
// Service implementation
public class OrderService : IOrderService
{
    private readonly HttpClient _httpClient;
    
    public OrderService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    
    public async Task<OrderDto> GetOrderAsync(Guid id)
    {
        // The HTTP client is already configured with resilience and service discovery
        return await _httpClient.GetFromJsonAsync<OrderDto>($"api/v1/orders/{id}");
    }
}

// Registration
services.AddHttpClient<IOrderService, OrderService>()
    .AddServiceDiscovery()
    .AddRestaurantResilienceHandler();
```

### Health Check Registration

```csharp
// Add custom health check
builder.Services.AddHealthChecks()
    .AddCheck("payment-processor", () => 
        _paymentProcessor.IsAvailable 
            ? HealthCheckResult.Healthy() 
            : HealthCheckResult.Degraded("Payment processor experiencing issues"),
        new[] { "services" });
```

## Contributing

When contributing to this project:

1. **Consistency**: Maintain consistent patterns across all components
2. **Backward Compatibility**: Avoid breaking changes to existing services
3. **Minimal Dependencies**: Keep external dependencies to a minimum
4. **Documentation**: Include XML comments and update this README
5. **Testing**: Add unit tests for all new components

### Adding a New Feature

1. Create the feature in the appropriate directory
2. Add extension methods for easy configuration
3. Update this README with usage examples
4. Add unit tests
5. Submit a pull request

### Required Skills

- Advanced .NET and ASP.NET Core knowledge
- Understanding of microservices architecture
- Experience with service discovery, health checks, and distributed systems
- Familiarity with observability concepts (metrics, tracing, logging)
