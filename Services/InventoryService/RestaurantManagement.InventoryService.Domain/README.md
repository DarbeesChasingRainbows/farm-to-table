# Inventory Domain Entities

This directory contains the core domain entities for the Inventory Management System. These entities represent the central business concepts of the inventory domain and encapsulate the business logic and rules.

## Entities Overview

| File | Description |
|------|-------------|
| `InventoryItem.cs` | Represents an item in inventory with its properties and stock levels |
| `StockLevel.cs` | Tracks the quantity of an inventory item at a specific location |
| `Location.cs` | Represents a physical storage location for inventory |
| `InventoryTransaction.cs` | Records all movements and changes to inventory |
| `Batch.cs` | Tracks batches of inventory items with expiration dates |
| `Vendor.cs` | Represents suppliers of inventory items |
| `CountSheet.cs` | Manages physical inventory counting process |

## Entity Details

### InventoryItem.cs

**Properties:**

- `Id` (string) - Unique identifier
- `Name` (string) - Item name
- `Description` (string) - Item description
- `SKU` (string) - Stock keeping unit
- `Category` (string) - Primary category
- `Subcategory` (string) - Secondary category
- `UnitOfMeasure` (string) - Unit of measure (kg, liter, each, etc.)
- `StorageRequirements` (string) - Storage requirements
- `ReorderThreshold` (double) - Level that triggers reordering
- `MinStockLevel` (double) - Minimum acceptable stock level
- `MaxStockLevel` (double) - Maximum desired stock level
- `LeadTimeDays` (int) - Typical reorder lead time in days
- `TrackExpiration` (bool) - Whether to track expiration
- `DefaultVendorId` (string) - Primary vendor ID
- `CostingMethod` (string) - Method used for costing (FIFO, LIFO, etc.)
- `LastCost` (decimal) - Last purchase cost
- `AverageCost` (decimal) - Average cost of all inventory
- `IsActive` (bool) - Whether the item is active
- `CreatedAt` (DateTime) - Creation timestamp
- `CreatedBy` (string) - User who created the item
- `LastModifiedAt` (DateTime?) - Last modification timestamp
- `LastModifiedBy` (string) - User who last modified the item
- `AlternativeItems` (IReadOnlyCollection<InventoryItem>) - Alternative items that can be substituted
- `StockLevels` (IReadOnlyCollection<StockLevel>) - Stock levels across locations
- `Batches` (IReadOnlyCollection<Batch>) - Batches of this item

**Methods:**

- `InventoryItem(string id, string name, string description, string sku, string category, string unitOfMeasure, double reorderThreshold, string createdBy)`
- `void UpdateDetails(string name, string description, string category, string subcategory, string storageRequirements, string modifiedBy)`
- `void UpdateUnitOfMeasure(string unitOfMeasure, string modifiedBy)`
- `void SetThresholds(double reorderThreshold, double minStockLevel, double maxStockLevel, int leadTimeDays, string modifiedBy)`
- `void SetExpirationTracking(bool trackExpiration, string modifiedBy)`
- `void SetDefaultVendor(string vendorId, string modifiedBy)`
- `void SetCostingMethod(string costingMethod, string modifiedBy)`
- `void AddAlternativeItem(InventoryItem item)`
- `void RemoveAlternativeItem(string itemId)`
- `void Discontinue(string modifiedBy)`
- `void Reactivate(string modifiedBy)`
- `void UpdateCost(decimal newCost, string modifiedBy)`
- `void UpdateAverageCost(decimal newAverageCost, string modifiedBy)`
- `StockLevel AddStockLevel(string locationId)`
- `void AddBatch(Batch batch)`
- `bool IsLowStock(string locationId)`
- `bool IsOutOfStock(string locationId)`
- `double GetTotalStockQuantity()`

### StockLevel.cs

**Properties:**

- `Id` (string) - Unique identifier
- `InventoryItemId` (string) - Reference to inventory item
- `LocationId` (string) - Reference to location
- `CurrentQuantity` (double) - Current physical quantity
- `ReservedQuantity` (double) - Quantity reserved for orders
- `LastUpdated` (DateTime) - Last update timestamp
- `InventoryItem` (InventoryItem) - Navigation property
- `AvailableQuantity` (double) - Computed property: CurrentQuantity - ReservedQuantity

**Methods:**

- `StockLevel(string id, string inventoryItemId, string locationId)`
- `void UpdateQuantity(double newQuantity)`
- `void IncreaseQuantity(double quantityToAdd)`
- `void DecreaseQuantity(double quantityToRemove)`
- `void Reserve(double quantity)`
- `void Release(double quantity)`
- `bool IsAvailable(double requiredQuantity)`

### Location.cs

**Properties:**

- `Id` (string) - Unique identifier
- `Name` (string) - Location name
- `Description` (string) - Location description
- `Type` (string) - Location type (storage, prep area, etc.)
- `MinTemperature` (double?) - Minimum storage temperature
- `MaxTemperature` (double?) - Maximum storage temperature
- `TargetHumidity` (double?) - Target humidity level
- `SpecialRequirements` (string) - Special storage requirements
- `Capacity` (double?) - Storage capacity
- `CapacityUnit` (string) - Unit of capacity measurement
- `IsActive` (bool) - Whether the location is active
- `CreatedAt` (DateTime) - Creation timestamp
- `CreatedBy` (string) - User who created the location
- `LastModifiedAt` (DateTime?) - Last modification timestamp
- `LastModifiedBy` (string) - User who last modified the location

**Methods:**

- `Location(string id, string name, string type)`
- `void UpdateDetails(string name, string description, string type, string modifiedBy)`
- `void UpdateStorageConditions(double? minTemperature, double? maxTemperature, double? targetHumidity, string specialRequirements, string modifiedBy)`
- `void UpdateCapacity(double? capacity, string capacityUnit, string modifiedBy)`
- `void Deactivate(string modifiedBy)`
- `void Activate(string modifiedBy)`

### InventoryTransaction.cs

**Properties:**

- `Id` (string) - Unique identifier
- `Type` (string) - Transaction type (received, consumed, etc.)
- `TransactionDate` (DateTime) - Date/time of transaction
- `ReferenceNumber` (string) - External reference number
- `ReferenceType` (string) - Reference type (PO, order, etc.)
- `LocationId` (string) - Source location ID
- `DestinationLocationId` (string) - Destination location ID for transfers
- `UserId` (string) - User who performed the transaction
- `Notes` (string) - Additional notes
- `Items` (IReadOnlyCollection<TransactionItem>) - Items involved in the transaction

**Methods:**

- `InventoryTransaction(string id, string type, DateTime transactionDate, string userId)`
- `void SetReference(string referenceNumber, string referenceType)`
- `void SetLocation(string locationId)`
- `void SetDestinationLocation(string destinationLocationId)`
- `void SetNotes(string notes)`
- `void AddItem(string itemId, double quantity, string locationId, string batchId = null, decimal? unitCost = null)`

**TransactionItem Class Properties:**

- `Id` (string) - Unique identifier
- `TransactionId` (string) - Parent transaction ID
- `InventoryItemId` (string) - Reference to inventory item
- `Quantity` (double) - Quantity transacted
- `LocationId` (string) - Location for this item
- `BatchId` (string) - Batch ID if applicable
- `UnitCost` (decimal?) - Unit cost if applicable
- `Transaction` (InventoryTransaction) - Navigation property
- `InventoryItem` (InventoryItem) - Navigation property

**TransactionItem Class Methods:**

- `TransactionItem(string id, string transactionId, string inventoryItemId, double quantity, string locationId, string batchId = null, decimal? unitCost = null)`

### Batch.cs

**Properties:**

- `Id` (string) - Unique identifier
- `InventoryItemId` (string) - Reference to inventory item
- `BatchNumber` (string) - Batch or lot number
- `ReceivedDate` (DateTime) - Date received
- `ExpirationDate` (DateTime) - Expiration date
- `InitialQuantity` (double) - Initial quantity received
- `RemainingQuantity` (double) - Remaining quantity
- `UnitCost` (decimal) - Cost per unit
- `LocationId` (string) - Current location ID
- `VendorId` (string) - Vendor ID
- `PurchaseOrderId` (string) - Purchase order ID
- `InventoryItem` (InventoryItem) - Navigation property

**Methods:**

- `Batch(string id, string inventoryItemId, string batchNumber, DateTime receivedDate, DateTime expirationDate, double initialQuantity, decimal unitCost, string locationId)`
- `void SetVendorInfo(string vendorId, string purchaseOrderId)`
- `void UpdateRemainingQuantity(double newRemainingQuantity)`
- `void Consume(double quantity)`
- `void Transfer(string newLocationId)`
- `bool IsExpired()`
- `bool IsExpiringSoon(int daysThreshold)`
- `int GetDaysUntilExpiration()`
- `decimal GetTotalValue()`

### Vendor.cs

**Properties:**

- `Id` (string) - Unique identifier
- `Name` (string) - Vendor name
- `ContactName` (string) - Contact person
- `Email` (string) - Email address
- `Phone` (string) - Phone number
- `Street` (string) - Street address
- `City` (string) - City
- `State` (string) - State/province
- `PostalCode` (string) - Postal code
- `Country` (string) - Country
- `AccountNumber` (string) - Account number with vendor
- `PaymentTerms` (string) - Payment terms
- `Notes` (string) - Additional notes
- `IsActive` (bool) - Whether the vendor is active
- `CreatedAt` (DateTime) - Creation timestamp
- `CreatedBy` (string) - User who created the vendor
- `LastModifiedAt` (DateTime?) - Last modification timestamp
- `LastModifiedBy` (string) - User who last modified the vendor
- `SuppliedItems` (IReadOnlyCollection<VendorItem>) - Items supplied by this vendor

**Methods:**

- `Vendor(string id, string name, string contactName, string email, string phone)`
- `void UpdateDetails(string name, string contactName, string email, string phone, string modifiedBy)`
- `void UpdateAddress(string street, string city, string state, string postalCode, string country, string modifiedBy)`
- `void UpdateAccountInfo(string accountNumber, string paymentTerms, string modifiedBy)`
- `void UpdateNotes(string notes, string modifiedBy)`
- `void Deactivate(string modifiedBy)`
- `void Activate(string modifiedBy)`
- `void AddSuppliedItem(string itemId, string vendorSku, decimal unitCost, string unitOfMeasure, double? minOrderQuantity, int leadTimeDays, bool isPreferred)`
- `void RemoveSuppliedItem(string itemId)`
- `VendorItem GetSuppliedItem(string itemId)`

**VendorItem Class Properties:**

- `Id` (string) - Unique identifier
- `VendorId` (string) - Parent vendor ID
- `ItemId` (string) - Inventory item ID
- `VendorSku` (string) - Vendor's item code
- `UnitCost` (decimal) - Cost per unit
- `UnitOfMeasure` (string) - Unit of measure
- `MinOrderQuantity` (double?) - Minimum order quantity
- `LeadTimeDays` (int) - Lead time in days
- `IsPreferred` (bool) - Whether this is the preferred vendor for this item
- `Vendor` (Vendor) - Navigation property
- `Item` (InventoryItem) - Navigation property

**VendorItem Class Methods:**

- `VendorItem(string id, string vendorId, string itemId, string vendorSku, decimal unitCost, string unitOfMeasure, double? minOrderQuantity, int leadTimeDays, bool isPreferred)`
- `void Update(string vendorSku, decimal unitCost, string unitOfMeasure, double? minOrderQuantity, int leadTimeDays, bool isPreferred)`

### CountSheet.cs

**Properties:**

- `Id` (string) - Unique identifier
- `LocationId` (string) - Location being counted
- `Status` (string) - Current status (Created, InProgress, etc.)
- `CountDate` (DateTime) - Scheduled count date
- `RequestedBy` (string) - User who requested the count
- `CountedBy` (string) - User performing the count
- `CompletedDate` (DateTime?) - Completion date
- `ApprovedBy` (string) - User who approved variances
- `ApprovalDate` (DateTime?) - Approval date
- `Notes` (string) - Additional notes
- `CreatedAt` (DateTime) - Creation timestamp
- `Items` (IReadOnlyCollection<CountSheetItem>) - Items to be counted
- `Categories` (IReadOnlyCollection<string>) - Categories included in count

**Methods:**

- `CountSheet(string id, string locationId, DateTime countDate, string requestedBy)`
- `void AddCategory(string category)`
- `void AddItem(string itemId, string itemName, string sku, string category, string unitOfMeasure, double systemQuantity)`
- `void SetNotes(string notes)`
- `void StartCounting(string countedBy)`
- `void RecordCount(string itemId, double countedQuantity, string batchId = null)`
- `void CompleteCounting(DateTime completedDate)`
- `void ApproveVariances(string approvedBy, DateTime approvalDate)`

**CountSheetItem Class Properties:**

- `Id` (string) - Unique identifier
- `CountSheetId` (string) - Parent count sheet ID
- `ItemId` (string) - Inventory item ID
- `ItemName` (string) - Item name
- `SKU` (string) - Stock keeping unit
- `Category` (string) - Item category
- `UnitOfMeasure` (string) - Unit of measure
- `SystemQuantity` (double) - Expected quantity in system
- `CountedQuantity` (double?) - Actual counted quantity
- `Variance` (double?) - Difference between system and counted
- `VariancePercentage` (double?) - Variance as percentage
- `UnitCost` (decimal) - Cost per unit
- `VarianceValue` (decimal?) - Value of variance
- `BatchCounts` (IReadOnlyCollection<BatchCount>) - Counts by batch
- `HasBeenCounted` (bool) - Whether item has been counted
- `VarianceApproved` (bool) - Whether variance has been approved
- `VarianceReasonCode` (string) - Reason for variance

**CountSheetItem Class Methods:**

- `CountSheetItem(string id, string countSheetId, string itemId, string itemName, string sku, string category, string unitOfMeasure, double systemQuantity)`
- `void UpdateSystemQuantity(double systemQuantity)`
- `void RecordCount(double countedQuantity)`
- `void RecordBatchCount(string batchId, double countedQuantity)`
- `void ApproveVariance(string reasonCode)`

**BatchCount Class Properties:**

- `Id` (string) - Unique identifier
- `CountSheetItemId` (string) - Parent count sheet item ID
- `BatchId` (string) - Batch ID
- `BatchNumber` (string) - Batch number
- `ExpirationDate` (DateTime) - Batch expiration date
- `SystemQuantity` (double) - Expected quantity in system
- `CountedQuantity` (double?) - Actual counted quantity

**BatchCount Class Methods:**

- `BatchCount(string id, string countSheetItemId, string batchId, string batchNumber, DateTime expirationDate, double systemQuantity)`
- `void UpdateSystemQuantity(double systemQuantity)`
- `void RecordCount(double countedQuantity)`

## Domain Rules and Invariants

1. **InventoryItem**
   - An item must have a unique SKU
   - An item cannot be an alternative to itself
   - Stock levels cannot go negative

2. **StockLevel**
   - Current quantity cannot be negative
   - Cannot reserve more than the available quantity
   - Cannot release more than the reserved quantity

3. **Batch**
   - Expiration date must be after received date
   - Remaining quantity cannot exceed initial quantity
   - Remaining quantity cannot be negative

4. **Location**
   - Location name must be unique
   - Minimum temperature cannot be greater than maximum temperature

5. **Vendor**
   - Vendor name must be unique
   - Each vendor can supply an item only once (but multiple vendors can supply the same item)

## Relationships

- **InventoryItem** ↔ **StockLevel**: One-to-many (an item can have multiple stock levels at different locations)
- **InventoryItem** ↔ **Batch**: One-to-many (an item can have multiple batches)
- **InventoryItem** ↔ **InventoryItem**: Many-to-many (items can be alternatives to each other)
- **Vendor** ↔ **InventoryItem**: Many-to-many (through VendorItem)
- **InventoryTransaction** → **TransactionItem**: One-to-many (a transaction can include multiple items)
- **CountSheet** → **CountSheetItem**: One-to-many (a count sheet includes multiple items)
- **CountSheetItem** → **BatchCount**: One-to-many (a count sheet item can have multiple batch counts)
