# Restaurant Management - Inventory Service API

## Overview

The API layer of the Restaurant Management Inventory Service provides external access to the system's functionality through multiple protocols. It serves as the interface between clients and the application's core business logic, ensuring proper request validation, error handling, and response formatting.

## Architecture

The API layer follows RESTful design principles and provides three distinct interfaces:

- **REST API**: Traditional HTTP-based API with JSON payloads
- **GraphQL API**: Flexible query language for complex data retrieval
- **gRPC API**: High-performance RPC framework for service-to-service communication

All interfaces adhere to the Clean Architecture pattern, utilizing the Mediator pattern to communicate with the application layer without direct coupling.

## Key Features

- Complete inventory management operations via multiple protocols
- Consistent error handling and validation
- Detailed API documentation via Swagger/OpenAPI
- Authentication and authorization
- Request logging and performance monitoring
- Health checks and diagnostics

## Technical Stack

- **Framework**: .NET 9.0
- **API Documentation**: Swagger/OpenAPI
- **GraphQL**: HotChocolate
- **gRPC**: gRPC for ASP.NET Core
- **Versioning**: Microsoft.AspNetCore.Mvc.Versioning
- **Logging**: Serilog
- **Messaging**: MediatR for CQRS pattern implementation

## API Endpoints

### REST API

The REST API follows standard HTTP conventions with the following resource endpoints:

#### Inventory Items

| Method | Endpoint                  | Description                          |
|--------|---------------------------|--------------------------------------|
| GET    | /api/inventoryitems       | Get all inventory items (paginated)  |
| GET    | /api/inventoryitems/{id}  | Get an inventory item by ID          |
| POST   | /api/inventoryitems       | Create a new inventory item          |
| PUT    | /api/inventoryitems/{id}  | Update an existing inventory item    |
| DELETE | /api/inventoryitems/{id}  | Discontinue an inventory item        |

#### Inventory Transactions

| Method | Endpoint                        | Description                     |
|--------|-------------------------------  |--------------------------------|
| GET    | /api/inventorytransactions      | Get all transactions (paginated)|
| GET    | /api/inventorytransactions/{id} | Get a transaction by ID         |
| POST   | /api/inventorytransactions/receive | Receive inventory           |
| POST   | /api/inventorytransactions/consume | Consume inventory           |
| POST   | /api/inventorytransactions/transfer | Transfer inventory         |
| POST   | /api/inventorytransactions/adjust | Adjust inventory quantities   |
| POST   | /api/inventorytransactions/waste | Record inventory waste        |

#### Count Sheets

| Method | Endpoint                      | Description                     |
|--------|-------------------------------|---------------------------------|
| GET    | /api/countsheets              | Get all count sheets (paginated)|
| GET    | /api/countsheets/{id}         | Get a count sheet by ID         |
| POST   | /api/countsheets/generate     | Generate a new count sheet      |
| POST   | /api/countsheets/{id}/record-counts | Record inventory counts   |
| POST   | /api/countsheets/{id}/approve-variances | Approve count variances |

#### Other Resources

- `GET /api/locations`: Get all locations
- `POST /api/locations`: Create a new location
- `GET /api/batches`: Get all batches
- `POST /api/batches`: Create a new batch
- `GET /api/vendors`: Get all vendors
- `GET /api/reports/value`: Get inventory valuation report
- `GET /api/reports/usage`: Get inventory usage report
- `GET /api/reports/expiring`: Get expiring inventory report

### GraphQL API

The GraphQL API is available at `/graphql` and provides a flexible query interface:

**Example Queries:**

```graphql
# Get inventory items with filtering
query {
  inventoryItems(
    search: "tomato", 
    category: "produce", 
    belowThreshold: true
  ) {
    items {
      id
      name
      sku
      category
      stockLevels {
        locationName
        currentQuantity
      }
    }
    totalItems
    pageNumber
    pageSize
  }
}

# Get inventory value with filtering
query {
  inventoryValue(
    locationIds: ["loc-1", "loc-2"],
    categoryIds: ["cat-1"]
  ) {
    totalValue
    valueByCategory {
      category
      value
      percentage
    }
    valueByLocation {
      locationName
      value
      percentage
    }
  }
}
```

**Example Mutations:**

```graphql
# Record waste
mutation {
  recordWaste(
    items: [
      {
        itemId: "item-1",
        quantity: 5.5,
        locationId: "loc-1",
        costPerUnit: 2.99
      }
    ],
    wasteReason: "Expired"
  ) {
    transactionId
    totalWasteCost
    success
  }
}
```

### gRPC Service

The gRPC service is defined in `inventory.proto` and provides high-performance methods for:

- `GetInventoryItem`: Retrieve item details by ID
- `CheckAvailability`: Verify stock availability for multiple items
- `ReceiveInventory`: Process inventory receipts

The service is accessible at the default gRPC port (usually 5001).

## Authentication & Authorization

The API uses JWT Bearer authentication with the following configuration:

```json
"Authentication": {
  "JwtBearer": {
    "Authority": "https://auth.restaurant.example.com",
    "Audience": "inventory-service",
    "RequireHttpsMetadata": true
  }
}
```

Authorization is role-based with the following main roles:

- **Admin**: Full access to all API endpoints
- **Manager**: Access to create, update and read operations
- **Staff**: Limited access to read operations and transactions
- **ReadOnly**: Access to read-only operations

## Error Handling

The API implements a global exception handling middleware that returns standardized error responses:

```json
{
  "status": 400,
  "title": "Validation error",
  "detail": "One or more validation errors occurred",
  "errors": {
    "Quantity": ["The field Quantity must be greater than 0."]
  }
}
```

Common HTTP status codes:

- `200 OK`: Successful operation
- `201 Created`: Resource created successfully
- `400 Bad Request`: Validation error
- `404 Not Found`: Resource not found
- `409 Conflict`: Business rule violation
- `500 Internal Server Error`: Unexpected error

## Performance Monitoring

The API includes:

- Request duration tracking
- Slow request logging (>500ms)
- Database query performance monitoring
- Application Insights integration (when enabled)

## Health Checks

Health endpoints are available at:

- `/health`: Overall service health
- `/health/ready`: Readiness check
- `/health/live`: Liveness check

## Cross-Origin Resource Sharing (CORS)

CORS is configured to allow specific origins:

```json
"CORS": {
  "AllowedOrigins": [
    "http://localhost:3000",
    "https://restaurant-management.example.com"
  ],
  "AllowedMethods": ["GET", "POST", "PUT", "DELETE", "OPTIONS"],
  "AllowedHeaders": ["*"]
}
```

## API Versioning

API versioning is supported through:

- URL versioning (e.g., `/api/v1/inventoryitems`)
- Header versioning (`api-version` header)
- Query string versioning (`?api-version=1.0`)

## Getting Started

### Prerequisites

- .NET 9.0 SDK
- SQL Server (or SQL Server Express)
- RabbitMQ (for messaging)
- Redis (optional, for caching)

### Configuration

1. Update connection strings in `appsettings.json`
2. Configure RabbitMQ settings
3. Configure authentication settings

### Running the API

```bash
# From the root directory
cd Services/InventoryService/RestaurantManagement.InventoryService.API
dotnet run
```

The API will be available at:

- REST API: <https://localhost:7043/api>
- GraphQL API: <https://localhost:7043/graphql>
- Swagger UI: <https://localhost:7043>

### Database Migrations

```bash
# Apply migrations
dotnet ef database update --project RestaurantManagement.InventoryService.Infrastructure --startup-project RestaurantManagement.InventoryService.API
```

## Testing

### Using Swagger UI

1. Navigate to `https://localhost:7043` in your browser
2. Explore and test available endpoints

### Using GraphQL Playground

1. Navigate to `https://localhost:7043/graphql` in your browser
2. Write and execute GraphQL queries

### Using gRPC Client

Use a gRPC client like [BloomRPC](https://github.com/uw-labs/bloomrpc) with the `inventory.proto` file.

## Logging

Logs are written to:

- Console (development)
- Structured log files (production)
- Application Insights (when configured)

Log settings are configurable in `appsettings.json`.

## Deployment

### Docker

A Dockerfile is provided for containerized deployment:

```dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["Services/InventoryService/RestaurantManagement.InventoryService.API/RestaurantManagement.InventoryService.API.csproj", "Services/InventoryService/RestaurantManagement.InventoryService.API/"]
# ... Copy other project files ...
RUN dotnet restore "Services/InventoryService/RestaurantManagement.InventoryService.API/RestaurantManagement.InventoryService.API.csproj"
COPY . .
WORKDIR "/src/Services/InventoryService/RestaurantManagement.InventoryService.API"
RUN dotnet build "RestaurantManagement.InventoryService.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "RestaurantManagement.InventoryService.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RestaurantManagement.InventoryService.API.dll"]
```

### Kubernetes

Example Kubernetes deployment:

```yaml
apiVersion: apps/v1
kind: Deployment
metadata:
  name: inventory-service
spec:
  replicas: 3
  selector:
    matchLabels:
      app: inventory-service
  template:
    metadata:
      labels:
        app: inventory-service
    spec:
      containers:
      - name: inventory-service
        image: restaurant-management/inventory-service:latest
        ports:
        - containerPort: 80
        - containerPort: 443
        env:
        - name: ASPNETCORE_ENVIRONMENT
          value: "Production"
        - name: ConnectionStrings__DefaultConnection
          valueFrom:
            secretKeyRef:
              name: inventory-service-secrets
              key: db-connection-string
```

## Development Guidelines

### Adding New Endpoints

1. Create a command/query in the Application layer
2. Create a controller method that uses the Mediator to send the command/query
3. Add appropriate response types and documentation
4. Add validation if necessary

### Versioning Strategy

- Increment minor version (e.g., 1.0 to 1.1) for backward-compatible changes
- Increment major version (e.g., 1.0 to 2.0) for breaking changes
- Maintain support for at least one previous major version

### Swagger Documentation

- Use XML comments to document controllers and models
- Include example requests and responses
- Document possible error responses

## Security Considerations

- All sensitive data is encrypted at rest and in transit
- API keys and secrets are stored in Azure Key Vault
- Authentication is required for all non-public endpoints
- Input validation is enforced for all requests
- Rate limiting is implemented to prevent abuse

## Contributing

1. Create a feature branch (`git checkout -b feature/amazing-feature`)
2. Commit your changes (`git commit -m 'Add some amazing feature'`)
3. Push to the branch (`git push origin feature/amazing-feature`)
4. Open a Pull Request

## API Conventions

- Use plural nouns for resource collections
- Use camelCase for JSON property names
- Use hyphens for URL paths
- Use GUID strings for resource IDs
- Return appropriate HTTP status codes
- Include pagination for collection endpoints
- Support filtering, sorting, and searching where appropriate

## Troubleshooting

### Common Issues

- **401 Unauthorized**: Check authentication token
- **403 Forbidden**: Verify user permissions
- **404 Not Found**: Check resource ID
- **409 Conflict**: Business rule violation
- **500 Internal Server Error**: Check logs for details

### Diagnostic Endpoints

- `/diagnostics/info`: Runtime information (development only)
- `/diagnostics/logs`: Recent log entries (development only)

## License

Copyright © 2025 Restaurant Management System
