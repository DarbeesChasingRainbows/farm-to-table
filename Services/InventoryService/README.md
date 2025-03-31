Restaurant Management - Inventory Service
Overview
The Inventory Service is a microservice component of the Restaurant Management system designed to handle all inventory-related operations. It provides robust capabilities for tracking, managing, and optimizing inventory across multiple restaurant locations.
Architecture
This service follows Clean Architecture principles:

Domain Layer: Core business entities and business rules
Application Layer: Use cases, application logic, and domain service interfaces
Infrastructure Layer: Implementation of interfaces defined in the application layer
API Layer: REST API endpoints for external consumption

Key Features

Complete inventory item management
Stock level tracking across multiple locations
Batch and expiration date management
Vendor relationship management
Inventory transactions (receipts, consumption, transfers)
Physical inventory counting
Low stock alerts and notifications
Expiration alerts and waste prevention
Reporting and analytics

Technical Stack

.NET 9.0: Framework for building the service
Entity Framework Core 9.0: ORM for database operations
SQL Server: Primary data store
MassTransit: Service bus implementation
RabbitMQ: Message broker for event-driven architecture

Domain Model
The domain model consists of the following key entities:

InventoryItem: Core entity representing items in inventory
StockLevel: Tracks quantity of an item at a specific location
Location: Represents physical storage locations
Batch: Manages batches with expiration dates
Vendor: Manages supplier information
InventoryTransaction: Records all inventory movements
CountSheet: Manages physical inventory counting

Domain Events
The service uses domain events to maintain loose coupling between components:

InventoryItemEvents: Creation, updates, threshold changes
StockLevelEvents: Quantity changes, reservations
BatchEvents: Creation, consumption, expiration
TransactionEvents: Completions, item movements

Infrastructure Components
Data Access

EntityFrameworkCore: Database context and entity configurations
Repositories: Data access abstraction per entity
UnitOfWork: Transaction coordination

Messaging

EventBus: Abstraction for publishing and subscribing to events
MassTransitEventBus: Implementation using MassTransit
EventConsumers:

LowStockEventConsumer
BatchExpiringSoonEventConsumer
InventoryTransactionCompletedConsumer

Services

CurrentUserService: Provides user context from HTTP requests
DateTimeService: Provides consistent date/time operations
NotificationService: Manages notifications across channels
EmailService: Handles email communications
FileStorageService: Manages document storage and retrieval
BarcodeService: Generates and reads item barcodes

Setup Instructions
Prerequisites

.NET 9.0 SDK
SQL Server (or SQL Server Express)
RabbitMQ server

Configuration

Update the connection strings in appsettings.json:

jsonCopy{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.;Database=RestaurantInventory;Trusted_Connection=True;MultipleActiveResultSets=true"
  },
  "RabbitMQ": {
    "Host": "localhost",
    "Username": "guest",
    "Password": "guest"
  },
  "EmailSettings": {
    "SmtpServer": "smtp.example.com",
    "Port": 587,
    "EnableSsl": true,
    "Username": "<notifications@example.com>",
    "Password": "your-password",
    "FromAddress": "<notifications@example.com>",
    "FromName": "Restaurant Inventory System"
  },
  "FileStorageSettings": {
    "BasePath": "D:\\RestaurantFiles",
    "BaseUrl": "<http://files.restaurant.example.com>",
    "InventoryDocumentsPath": "inventory-documents"
  }
}
Database Migrations
Apply migrations to create or update the database schema:
bashCopycd Services/InventoryService
dotnet ef migrations add InitialCreate --project RestaurantManagement.InventoryService.Infrastructure --startup-project RestaurantManagement.InventoryService.API
dotnet ef database update --project RestaurantManagement.InventoryService.Infrastructure --startup-project RestaurantManagement.InventoryService.API
Starting the Service
bashCopycd Services/InventoryService/RestaurantManagement.InventoryService.API
dotnet run
Project Structure
CopyRestaurantManagement.InventoryService/
├── Domain/
│   ├── Entities/
│   ├── Events/
│   └── Exceptions/
├── Application/
│   ├── Commands/
│   ├── Queries/
│   ├── Models/
│   └── Interfaces/
├── Infrastructure/
│   ├── Data/
│   │   ├── Context/
│   │   ├── EntityConfigurations/
│   │   ├── Repositories/
│   │   └── UnitOfWork/
│   ├── EventBus/
│   │   └── Consumers/
│   ├── Services/
│   └── DependencyInjection.cs
└── API/
    ├── Controllers/
    ├── Filters/
    ├── Middleware/
    └── Program.cs
Key Classes and Interfaces
Domain Layer

Domain Entities: Core business entities
Domain Events: Business events for domain changes
Domain Exceptions: Specialized exceptions for domain validation

Application Layer

Command Handlers: Processes commands to modify state
Query Handlers: Retrieves data without state modifications
Service Interfaces: Defines contracts for external services

Infrastructure Layer

Repository Classes: Implements data access operations
DbContext: Configures entity mappings and relationships
Event Consumers: Handles domain events for side effects
Service Implementations: Implements application service interfaces

Testing
bashCopycd Services/InventoryService
dotnet test
API Documentation
Once running, API documentation is available at:
Copyhttp://localhost:5000/swagger
Event Schema
All domain events follow a consistent schema:

EventId: Unique identifier for the event
EventType: Type of the event
Timestamp: When the event occurred
UserId: User who triggered the event
Payload: Event-specific data

Security
The service implements:

JWT authentication
Role-based authorization
Input validation
Security headers
HTTPS enforcement

Contributing

Create a feature branch (git checkout -b feature/amazing-feature)
Commit your changes (git commit -m 'Add some amazing feature')
Push to the branch (git push origin feature/amazing-feature)
Open a Pull Request
