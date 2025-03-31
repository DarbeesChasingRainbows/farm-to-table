# Inventory Service Infrastructure Layer

Overview
The Infrastructure layer of the Inventory Service provides concrete implementations of the interfaces defined in the Application layer. It contains data access, messaging, and external service integration components.

Core Components
Data Access
Context: Database context with entity configurations
Repositories: Data access implementations for each entity
UnitOfWork: Transaction coordination across repositories
Messaging
EventBus: Service bus implementation using MassTransit and RabbitMQ
EventPublisher: Publishes domain events to the message broker
Consumers: Handles domain events for specific business processes
Services
DateTimeService: Consistent date/time handling across the application
CurrentUserService: Extracts user context from HTTP requests
EmailService: Email sending capabilities for notifications
NotificationService: Multi-channel notification system (email, SMS, push)
FileStorageService: Document storage and retrieval
BarcodeService: Generation and reading of item barcodes
Key Files and Directories
Infrastructure/
├── Data/
│   ├── Context/
│   │   └── InventoryDbContext.cs
│   ├── EntityConfigurations/
│   │   ├── InventoryItemConfiguration.cs
│   │   ├── StockLevelConfiguration.cs
│   │   ├── LocationConfiguration.cs
│   │   ├── InventoryTransactionConfiguration.cs
│   │   ├── BatchConfiguration.cs
│   │   ├── VendorConfiguration.cs
│   │   └── CountSheetConfiguration.cs
│   ├── Repositories/
│   │   ├── Interfaces/
│   │   │   ├── IInventoryItemRepository.cs
│   │   │   ├── IStockLevelRepository.cs
│   │   │   ├── ILocationRepository.cs
│   │   │   ├── IInventoryTransactionRepository.cs
│   │   │   ├── IBatchRepository.cs
│   │   │   ├── IVendorRepository.cs
│   │   │   └── ICountSheetRepository.cs
│   │   ├── Repository.cs
│   │   ├── InventoryItemRepository.cs
│   │   ├── StockLevelRepository.cs
│   │   ├── LocationRepository.cs
│   │   ├── InventoryTransactionRepository.cs
│   │   ├── BatchRepository.cs
│   │   ├── VendorRepository.cs
│   │   └── CountSheetRepository.cs
│   └── UnitOfWork/
│       ├── IUnitOfWork.cs
│       └── UnitOfWork.cs
├── EventBus/
│   ├── IEventBus.cs
│   ├── MassTransitEventBus.cs
│   ├── EventPublisher.cs
│   └── Consumers/
│       ├── BaseConsumer.cs
│       ├── LowStockEventConsumer.cs
│       ├── BatchExpiringSoonEventConsumer.cs
│       └── InventoryTransactionCompletedConsumer.cs
├── Services/
│   ├── CurrentUserService.cs
│   ├── DateTimeService.cs
│   ├── EmailService.cs
│   ├── NotificationService.cs
│   ├── FileStorageService.cs
│   ├── BarcodeService.cs
│   └── Settings/
│       └── ServiceSettings.cs
└── DependencyInjection.cs
Database Configuration
The Infrastructure layer uses Entity Framework Core with SQL Server. Entity configurations define:

Primary keys and indexes
Data type mappings and constraints
Relationships between entities
Default values and computed columns
Repository Pattern
Each entity has a dedicated repository that implements:

Basic CRUD operations via the generic repository
Entity-specific query methods
Business rules validation
Optimized data access patterns
Unit of Work
The Unit of Work pattern provides:

Coordinated transactions across multiple repositories
Atomic operations for maintaining data consistency
Simplified transaction management for the application layer
Messaging Architecture
The messaging system is built on:

MassTransit: Service bus abstraction
RabbitMQ: Message broker implementation
Event Consumers: Handle domain events and perform business processes
Key message handling features:

Message retry policies
Delayed redelivery for transient failures
Dead letter queues for failed messages
Message serialization and deserialization
Event Consumers
Event consumers implement business processes triggered by domain events:

LowStockEventConsumer: Notifies appropriate personnel when items reach reorder thresholds
BatchExpiringSoonEventConsumer: Alerts about batches approaching expiration
InventoryTransactionCompletedConsumer: Processes inventory movements based on transaction type
External Services
The infrastructure layer integrates with external systems through service implementations:

Email Services: SMTP-based email sending
File Storage: Local or cloud-based document storage
Barcode Generation: Creates and reads inventory item barcodes
Dependency Injection
All infrastructure components are registered in the DependencyInjection.cs file, which:

Registers database context and repositories
Configures message bus and consumers
Registers service implementations
Sets up configuration options
Configuration
The infrastructure layer is configured through appSettings.json:

json
{
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
Usage
To use the Infrastructure layer:

Register services in your application's startup:
csharp
// In Program.cs or Startup.cs
builder.Services.AddInfrastructure(builder.Configuration);
Inject the repositories or UnitOfWork where needed:
csharp
public class InventoryService
{
    private readonly IUnitOfWork _unitOfWork;

    public InventoryService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<InventoryItem> GetInventoryItemAsync(string id)
    {
        return await _unitOfWork.InventoryItems.GetByIdAsync(id);
    }
}
Entity Framework Migrations
Create and apply migrations:

bash
dotnet ef migrations add InitialCreate --project RestaurantManagement.InventoryService.Infrastructure --startup-project RestaurantManagement.InventoryService.API
dotnet ef database update --project RestaurantManagement.InventoryService.Infrastructure --startup-project RestaurantManagement.InventoryService.API
