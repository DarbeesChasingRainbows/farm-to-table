# Domain Events

This directory contains domain event classes for the Inventory Management System. Domain events represent significant state changes within the system that other components might need to react to. They help maintain a loosely coupled architecture by allowing different parts of the system to communicate without direct dependencies.

## Core Components

| File | Description |
|------|-------------|
| `IDomainEvent.cs` | Interface that all domain events implement |
| `BaseDomainEvent.cs` | Base class for all domain events providing common properties |
| `EventRegistrar.cs` | Handles the registration and publishing of domain events |
| `IEventHandler.cs` | Interface for components that handle domain events |
| `DomainEventsRegister.cs` | Registry of all domain events in the system |

## Entity-Specific Event Categories

Each major entity in the domain has its own set of events:

| File | Description |
|------|-------------|
| `InventoryItemEvents.cs` | Events related to inventory items (creation, updates, threshold changes) |
| `StockLevelEvents.cs` | Events related to stock level changes (quantity updates, reservations) |
| `LocationEvents.cs` | Events related to storage locations (creation, updates, activation status) |
| `InventoryTransactionEvents.cs` | Events related to inventory transactions (movements, consumption) |
| `BatchEvents.cs` | Events related to batch tracking (creation, consumption, expiration) |
| `VendorEvents.cs` | Events related to vendors (creation, updates, supplied items) |
| `CountSheetEvents.cs` | Events related to physical inventory counting process |

## Usage Patterns

Domain events follow these usage patterns:

1. **Event Creation**: Events are created by domain entities when significant state changes occur
2. **Event Registration**: Events are registered with the `EventRegistrar`
3. **Event Publishing**: Events are published to interested handlers
4. **Event Handling**: Handlers respond to events based on business requirements

## Event Structure

All domain events:

- Implement the `IDomainEvent` interface (via `BaseDomainEvent`)
- Include a timestamp indicating when the event occurred
- Include the ID of the user who triggered the event
- Include IDs of relevant entities affected by the event
- Include all relevant data about the state change

## Event Naming Conventions

Events are named according to these conventions:

- Use past tense to indicate the event has already occurred (e.g., `InventoryItemCreated`)
- Start with the entity name to make the event source clear
- End with "Event" to clearly identify the class as a domain event

## Event Examples

```csharp
// Event indicating an inventory item was created
public class InventoryItemCreatedEvent : InventoryItemEvent
{
    public string Name { get; }
    public string Description { get; }
    public string SKU { get; }
    public string Category { get; }
    public string UnitOfMeasure { get; }
    public double ReorderThreshold { get; }

    public InventoryItemCreatedEvent(
        string itemId, 
        string name, 
        string description, 
        string sku, 
        string category, 
        string unitOfMeasure, 
        double reorderThreshold, 
        string userId) : base(itemId, userId)
    {
        Name = name;
        Description = description;
        SKU = sku;
        Category = category;
        UnitOfMeasure = unitOfMeasure;
        ReorderThreshold = reorderThreshold;
    }
}

// Event indicating stock quantity was updated
public class StockLevelQuantityUpdatedEvent : StockLevelEvent
{
    public double OldQuantity { get; }
    public double NewQuantity { get; }

    public StockLevelQuantityUpdatedEvent(
        string stockLevelId, 
        string inventoryItemId, 
        string locationId, 
        double oldQuantity, 
        double newQuantity, 
        string userId) : base(stockLevelId, inventoryItemId, locationId, userId)
    {
        OldQuantity = oldQuantity;
        NewQuantity = newQuantity;
    }
}
```

## Integration with Domain Entities

Domain entities raise events through the following process:

1. Entities have a collection of domain events
2. When state changes occur, entities add appropriate events to this collection
3. When entities are saved, events are published and the collection is cleared

Example integration with entity:

```csharp
public class InventoryItem
{
    private readonly List<IDomainEvent> _domainEvents = new List<IDomainEvent>();
    
    // Entity properties and methods...
    
    public void UpdateQuantity(double newQuantity, string userId)
    {
        var oldQuantity = CurrentQuantity;
        CurrentQuantity = newQuantity;
        
        _domainEvents.Add(new InventoryItemQuantityUpdatedEvent(
            Id, oldQuantity, newQuantity, userId));
    }
    
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();
    
    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }
}
```
