# Restaurant Management System - Frontend Architecture

This repository is part of a larger microservices-based Restaurant Management System and contains the Inventory Management frontend application built with Deno Fresh. The system consists of multiple dedicated frontend applications, each focusing on specific business domains.

## System Overview

The Restaurant Management System is composed of the following dedicated frontend applications:

1. **Inventory Management Frontend** (This Repository)
   - Track and manage inventory across locations
   - Process receiving, transfers, and consumption
   - Monitor stock levels and thresholds

2. **Coffee Shop Kiosk Frontend** (Separate Repository)
   - Customer-facing self-service ordering interface
   - Menu display with categories and product details
   - Order customization and payment processing

3. **Order Processing Frontend** (Separate Repository)
   - Staff order management interface
   - Order creation, modification, and tracking
   - Payment handling and receipt generation

4. **Recipe Management Frontend** (Separate Repository)
   - Recipe creation and modification
   - Ingredient management and costing
   - Nutritional information and preparation instructions

5. **Kitchen Display System Frontend** (Separate Repository)
   - Real-time order visualization for kitchen staff
   - Preparation timing and station management
   - Order status updates and notifications

Each frontend application is designed to be standalone, focusing on its specific domain while communicating with shared backend microservices through well-defined APIs.

## Inventory Management Frontend

This application serves as the interface for inventory management operations within the Restaurant Management System.

### Technology Stack

- **[Deno](https://deno.land/)** - A secure runtime for JavaScript and TypeScript
- **[Fresh](https://fresh.deno.dev/)** - The next-gen web framework for Deno
- **[Preact](https://preactjs.com/)** - Fast 3kB alternative to React with the same modern API
- **[Signals](https://preactjs.com/guide/v10/signals/)** - Fine-grained reactivity for state management
- **[Tailwind CSS](https://tailwindcss.com/)** - A utility-first CSS framework
- **[DaisyUI](https://daisyui.com/)** - Component library for Tailwind CSS

### Architecture

The application architecture follows Domain-Driven Design principles with CQRS pattern implementation:

#### Domain Layer

- Contains the core domain models and business logic
- Defines interfaces and types that represent real-world entities
- Located in `domain/` directory

#### Application Layer

- Implements use cases through commands and queries (CQRS)
- Manages application state using signals
- Orchestrates domain objects and services
- Located in `application/` directory

#### Infrastructure Layer

- Handles external concerns like API communication
- Manages persistence, authentication, and technical capabilities
- Located in `infrastructure/` directory

#### Presentation Layer

- UI components and layouts
- Routes and page definitions
- Interactive islands
- Located in `components/`, `islands/`, and `routes/` directories

### CQRS Implementation

The application implements the Command Query Responsibility Segregation pattern:

#### Commands

- Represent intentions to change the system state
- Follow a one-way flow with no return values (apart from success/failure)
- Examples: `ReceiveInventoryCommand`, `TransferInventoryCommand`
- Located in `application/commands/` directory

#### Queries

- Retrieve data from the system
- Do not change state
- Optimized for reading and presenting data
- Examples: `GetInventoryItemsQuery`, `GetInventoryTransactionsQuery`
- Located in `application/queries/` directory

#### Command and Query Buses

- Central dispatchers for commands and queries
- Register handlers for specific command and query types
- Provide a clean API for executing operations
- Located in `application/commands/CommandBus.ts` and `application/queries/QueryBus.ts`

### State Management

The application uses Preact Signals for reactive state management:

- **State Signals** - Reactive state containers that trigger updates when changed
- **Computed Values** - Derived state that updates automatically when dependencies change
- **Actions** - Functions that modify state signals

This approach provides fine-grained reactivity without complex state management libraries.

### Folder Structure

```plaintext
inventory-management-frontend/
├── components/                # UI components
│   ├── common/                # Shared components
│   └── domains/               # Domain-specific components
│       └── inventory/         # Inventory components
├── routes/                    # Fresh routes (pages)
│   ├── api/                   # API routes (BFF)
│   ├── admin/                 # Admin UI routes
│   ├── _app.tsx               # App layout
│   └── index.tsx              # Home page
├── islands/                   # Interactive components
│   └── admin/                 # Admin interactive components
├── domain/                    # Domain layer
│   ├── models/                # Domain models
│   ├── events/                # Domain events
│   └── services/              # Domain services
├── application/               # Application layer
│   ├── commands/              # CQRS command definitions & handlers
│   ├── queries/               # CQRS query definitions & handlers
│   ├── services/              # Application services
│   └── state/                 # Signal-based state management
├── infrastructure/            # Infrastructure layer
│   ├── api/                   # API clients
│   ├── persistence/           # Local storage adapters
│   └── auth/                  # Authentication services
├── static/                    # Static assets
├── dev.ts                     # Development entry point
├── main.ts                    # Production entry point
├── twind.config.ts            # Tailwind configuration
└── deno.json                  # Deno configuration
```

### Key Features

#### Inventory Management

- Track inventory levels across locations
- Receive, transfer, and consume inventory
- Monitor low stock items and thresholds
- View inventory history and transactions
- Generate inventory reports and analytics
- Manage vendors and purchase orders
- Track batch information and expiration dates
- Set up and monitor automatic reordering thresholds

## Getting Started

### Prerequisites

- [Deno](https://deno.land/#installation) installed on your machine
- A connection to the Restaurant Management backend services

### Installation

1. Clone the repository:

   ```powershell
   git clone https://github.com/your-org/inventory-management-frontend.git
   cd inventory-management-frontend
   ```

2. Start the development server:

   ```powershell
   deno task start
   ```

3. Open your browser and navigate to [http://localhost:8000](http://localhost:8000)

### Development Commands

- `deno task start` - Start the development server
- `deno task build` - Build the production version
- `deno task preview` - Preview the production build locally

## API Integration

The frontend integrates with the backend services via:

1. **BFF (Backend for Frontend) Pattern**:
   - API routes in the `routes/api/` directory
   - Server-side requests to backend services
   - Data transformation and aggregation

2. **Direct API Calls**:
   - Command handlers dispatch requests to backend endpoints
   - Query handlers fetch data from backend services
   - Authentication tokens are managed and passed with requests

## Authentication and Authorization

The application uses token-based authentication with JWT:

- Authentication state is managed via signals
- Protected routes require authentication
- Role-based access controls determine feature availability
- Tokens are refreshed automatically when needed

## Deployment

This application can be deployed to:

- **Deno Deploy**: Simple one-command deployment

  ```powershell
  deployctl deploy --project=inventory-management main.ts
  ```

- **Docker**:

  ```powershell
  docker build -t inventory-management-frontend .
  docker run -p 8000:8000 inventory-management-frontend
  ```

- **Cloud Providers**: AWS, Azure, or GCP using Docker containers

## Communication with Other Frontends

While this application is standalone, it may need to communicate or integrate with other frontends in the system. This is achieved through:

1. **Shared Authentication** - Common authentication mechanism across frontends
2. **Deep Linking** - Links to specific pages in other applications when needed
3. **Event Communication** - Browser-based events for cross-application needs
4. **Backend Communication** - Interaction via backend services when required

## Frontend Applications in the System

### Coffee Shop Kiosk Frontend

- Customer-facing application for self-service ordering
- Repository: [github.com/your-org/coffee-shop-kiosk](https://github.com/your-org/coffee-shop-kiosk)

### Order Processing Frontend

- Staff interface for order management
- Repository: [github.com/your-org/order-processing-frontend](https://github.com/your-org/order-processing-frontend)

### Recipe Management Frontend

- Interface for creating and managing recipes
- Repository: [github.com/your-org/recipe-management-frontend](https://github.com/your-org/recipe-management-frontend)

### Kitchen Display System Frontend

- Real-time display for kitchen staff
- Repository: [github.com/your-org/kitchen-display-frontend](https://github.com/your-org/kitchen-display-frontend)

## Contributing

### Development Workflow

1. Create a feature branch from `develop`
2. Make your changes
3. Run tests
4. Submit a pull request to `develop`

### Coding Standards

- Follow TypeScript best practices
- Use Deno's formatting tools (`deno fmt`)
- Maintain the DDD and CQRS architecture
- Use signals for state management
- Document new components and features

## Inventory Management System - Deep Dive

## System Purpose

The Inventory Management System serves as the central hub for tracking, managing, and optimizing all inventory-related operations within the restaurant management ecosystem. It provides real-time visibility into inventory levels, facilitates inventory transactions, ensures stock accuracy, and helps prevent stockouts while minimizing waste and overstock situations.

## Core Domains and Concepts

### Inventory Items

At the heart of the system are inventory items, representing all products and ingredients used in the restaurant.

**Key Properties:**

- **Basic Information**: ID, name, description, SKU
- **Categorization**: Category, subcategory, tags
- **Measurement**: Unit of measure (kg, liter, each, etc.)
- **Storage Requirements**: Temperature, humidity, shelf-life
- **Ordering Parameters**: Reorder threshold, min/max stock levels, lead time
- **Substitutions**: Alternative items that can be used if unavailable
- **Costing**: Purchase cost, average cost, last cost

**Domain Model:**

```typescript
export interface InventoryItem {
  id: string;
  name: string;
  description: string;
  sku: string;
  category: string;
  subcategory: string;
  unitOfMeasure: string;
  storageRequirements: StorageRequirement;
  reorderThreshold: number;
  minStockLevel: number;
  maxStockLevel: number;
  leadTimeDays: number;
  trackExpiration: boolean;
  defaultVendorId: string;
  alternativeItemIds: string[];
  costingMethod: CostingMethod;
  lastCost: number;
  averageCost: number;
  stockLevels: StockLevel[];
  isActive: boolean;
  createdAt: string;
  updatedAt: string;
}
```

### Stock Levels

Stock levels represent the quantity of each inventory item at specific locations.

**Key Properties:**

- **Quantity Tracking**: Current quantity, reserved quantity, available quantity
- **Location**: Where the item is stored
- **Batch Tracking**: For items with expiration dates or lot numbers
- **History**: Timestamp of last update, update history

**Domain Model:**

```typescript
export interface StockLevel {
  itemId: string;
  locationId: string;
  currentQuantity: number;
  reservedQuantity: number;
  availableQuantity: number; // Computed: currentQuantity - reservedQuantity
  lastUpdated: string;
}
```

### Inventory Transactions

All movements of inventory are tracked as transactions.

**Transaction Types:**

- **Receiving**: Items received from vendors
- **Consumption**: Items used in preparation or sales
- **Transfer**: Items moved between locations
- **Adjustment**: Manual corrections to inventory counts
- **Waste**: Items discarded due to spoilage, damage, etc.

**Key Properties:**

- **Transaction Details**: Type, date/time, affected items and quantities
- **Reference Information**: Related documents (purchase orders, orders, etc.)
- **Personnel**: Who performed or authorized the transaction
- **Notes**: Additional context or explanation

**Domain Model:**

```typescript
export interface InventoryTransaction {
  id: string;
  type: 'RECEIVED' | 'CONSUMED' | 'TRANSFERRED' | 'ADJUSTED' | 'WASTED';
  items: TransactionItem[];
  timestamp: string;
  referenceNumber?: string;
  referenceType?: string;
  locationId: string;
  destinationLocationId?: string;
  userId: string;
  notes?: string;
}

export interface TransactionItem {
  itemId: string;
  quantity: number;
  unitCost?: number;
  batchId?: string;
}
```

### Batches and Expiration Tracking

For items with expiration dates or lot tracking requirements.

**Key Properties:**

- **Batch Information**: Batch/lot number, received date, expiration date
- **Quantity Tracking**: Initial quantity, remaining quantity
- **Cost Information**: Cost basis for this specific batch
- **Location**: Where the batch is stored

**Domain Model:**

```typescript
export interface Batch {
  id: string;
  itemId: string;
  batchNumber: string;
  receivedDate: string;
  expirationDate: string;
  initialQuantity: number;
  remainingQuantity: number;
  unitCost: number;
  locationId: string;
  vendorId?: string;
  purchaseOrderId?: string;
}
```

### Vendors

Suppliers of inventory items.

**Key Properties:**

- **Vendor Information**: Name, contact details, account number
- **Items Supplied**: Which inventory items they provide
- **Terms**: Payment terms, delivery schedule, minimum orders
- **Performance**: Reliability, quality, pricing history

**Domain Model:**

```typescript
export interface Vendor {
  id: string;
  name: string;
  contactName: string;
  email: string;
  phone: string;
  address: Address;
  accountNumber?: string;
  suppliedItems: VendorItem[];
  paymentTerms: string;
  notes?: string;
  isActive: boolean;
}

export interface VendorItem {
  itemId: string;
  vendorSku: string;
  unitCost: number;
  unitOfMeasure: string;
  minOrderQuantity?: number;
  leadTimeDays: number;
  isPreferred: boolean;
}
```

### Locations

Physical storage locations for inventory.

**Key Properties:**

- **Location Details**: Name, type (storage, prep area, bar, etc.)
- **Capacity**: Storage capacity and constraints
- **Conditions**: Temperature range, humidity, etc.

**Domain Model:**

```typescript
export interface Location {
  id: string;
  name: string;
  description?: string;
  type: LocationType;
  storageConditions?: StorageCondition;
  capacity?: number;
  capacityUnit?: string;
  isActive: boolean;
}
```

## Core Functionality

### Inventory Receiving

The process of recording inventory received from vendors.

**Process Flow:**

1. User selects items being received, with quantities and costs
2. System validates against purchase orders (if applicable)
3. Batch/lot information and expiration dates are recorded (if applicable)
4. Stock levels are updated
5. Cost basis is updated based on costing method
6. Transaction record is created
7. Alerts are generated for discrepancies

**Commands:**

- `ReceiveInventoryCommand`

**Queries:**

- `GetPendingReceivingsQuery`
- `GetReceivingHistoryQuery`

### Inventory Consumption

The process of recording inventory used in preparation or sales.

**Process Flow:**

1. Consumption can be triggered manually or automatically from orders
2. System checks availability of required items
3. For batch-tracked items, FIFO or FEFO (First Expired, First Out) is applied
4. Stock levels are updated
5. Transaction record is created
6. Low stock alerts are triggered if thresholds are crossed

**Commands:**

- `ConsumeInventoryCommand`

**Queries:**

- `GetConsumptionHistoryQuery`

### Inventory Transfers

The process of moving inventory between locations.

**Process Flow:**

1. User selects items to transfer, source and destination locations
2. System validates availability at source location
3. Stock levels are updated at both locations
4. Transaction record is created

**Commands:**

- `TransferInventoryCommand`

**Queries:**

- `GetTransferHistoryQuery`

### Inventory Counting and Reconciliation

The process of physically counting inventory and reconciling with system records.

**Process Flow:**

1. Count sheets are generated for specific locations/categories
2. Physical counts are recorded
3. System calculates variances between counted and expected quantities
4. Variances are reviewed and approved
5. Adjustment transactions are created
6. Stock levels are updated

**Commands:**

- `CreateCountSheetCommand`
- `RecordCountsCommand`
- `ApproveVariancesCommand`

**Queries:**

- `GetActiveCountSheetsQuery`
- `GetCountHistoryQuery`
- `GetVarianceReportQuery`

### Automatic Reordering

The system can suggest or automatically create purchase orders based on inventory levels.

**Process Flow:**

1. System regularly checks stock levels against reorder thresholds
2. For items below threshold, reorder quantity is calculated
3. Items are grouped by vendor
4. Suggested purchase orders are created
5. User reviews and approves purchase orders
6. Approved purchase orders are sent to vendors

**Commands:**

- `GenerateReorderSuggestionsCommand`
- `ApproveReorderCommand`

**Queries:**

- `GetReorderSuggestionsQuery`

### Waste Management

The process of recording inventory lost to spoilage, damage, or other waste.

**Process Flow:**

1. User records wasted items with quantities and reason codes
2. Stock levels are updated
3. Transaction record is created
4. Waste is categorized for reporting and analysis

**Commands:**

- `RecordWasteCommand`

**Queries:**

- `GetWasteReportQuery`

## Advanced Features

### Cost Management

The system tracks and manages inventory costs using various methods.

**Costing Methods:**

- **FIFO (First In, First Out)**: Items received first are consumed first
- **LIFO (Last In, First Out)**: Items received last are consumed first
- **Weighted Average**: Average cost across all inventory of an item
- **Specific Identification**: Tracking cost of specific units or batches

**Cost Analysis:**

- Historical cost trending
- Cost variance reporting
- Impact of price changes on recipe and menu costs

### Vendor Management

The system helps manage relationships with suppliers.

**Features:**

- Vendor performance tracking (on-time delivery, order accuracy)
- Price comparison across vendors
- Order history and spending analysis
- Preferred vendor designation

### Batch and Expiration Management

For items with limited shelf life or batch tracking requirements.

**Features:**

- FEFO (First Expired, First Out) consumption logic
- Expiration alerts and reporting
- Batch traceability for food safety and recalls
- Shelf-life analysis for minimizing waste

### Inventory Optimization

Tools for optimizing inventory levels.

**Features:**

- Economic order quantity (EOQ) calculation
- Safety stock determination
- Seasonal demand forecasting
- Inventory turnover analysis
- Dead stock identification

## User Interface Features

### Dashboard

The main control center providing at-a-glance information.

**Components:**

- Low stock alerts
- Expiring inventory warnings
- Recent transactions summary
- Inventory valuation
- Key performance indicators

### Inventory Browser

Interface for browsing and filtering inventory items.

**Features:**

- Multi-criteria filtering (category, location, status)
- Sort by various attributes
- Quick view of stock levels across locations
- Batch and expiration information
- Cost and valuation data

### Transaction Management

Interfaces for creating and managing inventory transactions.

**Features:**

- Guided flows for different transaction types
- Barcode scanning support
- Bulk operations
- Transaction history and audit trail

### Reports and Analytics

Comprehensive reporting capabilities.

**Standard Reports:**

- Inventory Valuation Report
- Stock Level Report
- Transaction History Report
- Variance Report
- Waste Report
- Expiration Report
- Vendor Performance Report
- Usage Analysis Report

**Analytics:**

- Trend analysis for usage patterns
- Cost variation over time
- Seasonality detection
- Waste reduction opportunities

## Integration Points

### Recipe Management System

- Provides ingredient requirements for recipes
- Receives inventory availability information
- Updates recipe costs based on ingredient costs

### Order Processing System

- Triggers inventory consumption when orders are placed
- Receives availability information for menu items

### Kitchen Display System

- Receives information about inventory constraints
- Provides feedback on actual usage vs. expected usage

### Coffee Shop Kiosk

- Receives real-time product availability information
- Updates menu items based on inventory status

### Reporting & Analytics System

- Receives inventory data for comprehensive business reporting
- Provides insights for inventory optimization

## Implementation Details

### CQRS Implementation for Inventory

#### Key Commands

```typescript
// ReceiveInventoryCommand
export interface ReceiveInventoryPayload {
  purchaseOrderId?: string;
  items: Array<{
    itemId: string;
    quantity: number;
    locationId: string;
    unitCost?: number;
    batchNumber?: string;
    expirationDate?: string;
  }>;
  notes?: string;
}

// ConsumeInventoryCommand
export interface ConsumeInventoryPayload {
  items: Array<{
    itemId: string;
    quantity: number;
    locationId: string;
    batchIds?: string[];
  }>;
  referenceId?: string;
  referenceType?: string;
  notes?: string;
}

// TransferInventoryCommand
export interface TransferInventoryPayload {
  items: Array<{
    itemId: string;
    quantity: number;
    sourceLocationId: string;
    destinationLocationId: string;
    batchIds?: string[];
  }>;
  notes?: string;
}

// AdjustInventoryCommand
export interface AdjustInventoryPayload {
  items: Array<{
    itemId: string;
    locationId: string;
    currentQuantity: number;
    newQuantity: number;
    reasonCode: string;
    batchId?: string;
  }>;
  notes?: string;
}
```

#### Key Queries

```typescript
// GetInventoryItemsQuery
export interface GetInventoryItemsParameters {
  locationId?: string;
  category?: string;
  search?: string;
  includeInactive?: boolean;
  page?: number;
  pageSize?: number;
}

// GetStockLevelsQuery
export interface GetStockLevelsParameters {
  itemIds?: string[];
  locationId?: string;
  belowThreshold?: boolean;
  includeZeroStock?: boolean;
  page?: number;
  pageSize?: number;
}

// GetInventoryTransactionsQuery
export interface GetInventoryTransactionsParameters {
  itemId?: string;
  locationId?: string;
  transactionType?: string;
  startDate?: string;
  endDate?: string;
  referenceId?: string;
  page?: number;
  pageSize?: number;
}

// GetExpiringInventoryQuery
export interface GetExpiringInventoryParameters {
  daysToExpiration: number;
  locationId?: string;
  categoryId?: string;
}
```

### State Management with Signals

The application uses fine-grained reactive state management with signals:

```typescript
// State signals
export const inventoryItems = signal<InventoryItem[]>([]);
export const locations = signal<Location[]>([]);
export const selectedLocationId = signal<string | null>(null);
export const selectedCategory = signal<string | null>(null);
export const searchQuery = signal<string>("");
export const isLoading = signal<boolean>(false);
export const error = signal<string | null>(null);

// Computed values
export const filteredItems = computed(() => {
  let items = inventoryItems.value;
  
  if (selectedCategory.value) {
    items = items.filter(item => item.category === selectedCategory.value);
  }
  
  if (searchQuery.value) {
    const query = searchQuery.value.toLowerCase();
    items = items.filter(item => 
      item.name.toLowerCase().includes(query) || 
      item.sku.toLowerCase().includes(query) ||
      item.description.toLowerCase().includes(query)
    );
  }
  
  return items;
});

// Other computed values for different views and metrics
export const lowStockItems = computed(() => {
  if (!selectedLocationId.value) return [];
  
  return inventoryItems.value.filter(item => {
    const stockLevel = item.stockLevels.find(sl => 
      sl.locationId === selectedLocationId.value
    );
    return stockLevel && stockLevel.availableQuantity < item.reorderThreshold;
  });
});

export const expiringItems = computed(() => {
  const today = new Date();
  const thirtyDaysFromNow = new Date();
  thirtyDaysFromNow.setDate(today.getDate() + 30);
  
  return inventoryItems.value.filter(item => {
    if (!item.batches) return false;
    
    return item.batches.some(batch => {
      const expirationDate = new Date(batch.expirationDate);
      return expirationDate <= thirtyDaysFromNow && batch.remainingQuantity > 0;
    });
  });
});

export const inventoryValue = computed(() => {
  return inventoryItems.value.reduce((total, item) => {
    const itemTotal = item.stockLevels.reduce((itemSum, level) => {
      return itemSum + (level.currentQuantity * item.averageCost);
    }, 0);
    return total + itemTotal;
  }, 0);
});
```

### UI Components

Key UI components for the inventory management system:

#### InventoryDashboard

```tsx
export default function InventoryDashboard() {
  const totalValue = useComputed(() => inventoryValue.value.toFixed(2));
  
  useEffect(() => {
    loadLocations().then(() => loadInventoryItems());
  }, []);
  
  return (
    <div class="flex flex-col gap-6">
      <h1 class="text-3xl font-bold">Inventory Dashboard</h1>
      
      <div class="stats shadow">
        <div class="stat">
          <div class="stat-title">Total Inventory Value</div>
          <div class="stat-value">${totalValue.value}</div>
        </div>
        
        <div class="stat">
          <div class="stat-title">Low Stock Items</div>
          <div class="stat-value text-warning">{lowStockItems.value.length}</div>
        </div>
        
        <div class="stat">
          <div class="stat-title">Expiring Soon</div>
          <div class="stat-value text-error">{expiringItems.value.length}</div>
        </div>
      </div>
      
      {/* Additional dashboard components */}
    </div>
  );
}
```

#### InventoryItemsTable

```tsx
export default function InventoryItemsTable() {
  useEffect(() => {
    loadInventoryItems();
  }, [selectedLocationId.value, selectedCategory.value, searchQuery.value]);
  
  return (
    <div class="overflow-x-auto">
      <table class="table w-full">
        <thead>
          <tr>
            <th>Name</th>
            <th>SKU</th>
            <th>Category</th>
            <th>In Stock</th>
            <th>Reserved</th>
            <th>Available</th>
            <th>Threshold</th>
            <th>Status</th>
            <th>Actions</th>
          </tr>
        </thead>
        <tbody>
          {filteredItems.value.map(item => {
            const stockLevel = item.stockLevels.find(sl => 
              sl.locationId === selectedLocationId.value
            ) || { currentQuantity: 0, reservedQuantity: 0, availableQuantity: 0 };
            
            const isLowStock = stockLevel.availableQuantity < item.reorderThreshold;
            
            return (
              <tr key={item.id} class={isLowStock ? "bg-warning bg-opacity-10" : ""}>
                <td>{item.name}</td>
                <td>{item.sku}</td>
                <td>{item.category}</td>
                <td>{stockLevel.currentQuantity} {item.unitOfMeasure}</td>
                <td>{stockLevel.reservedQuantity} {item.unitOfMeasure}</td>
                <td>{stockLevel.availableQuantity} {item.unitOfMeasure}</td>
                <td>{item.reorderThreshold} {item.unitOfMeasure}</td>
                <td>
                  {isLowStock ? 
                    <span class="badge badge-warning">Low Stock</span> : 
                    <span class="badge badge-success">OK</span>
                  }
                </td>
                <td>
                  <div class="flex gap-2">
                    <button 
                      class="btn btn-sm btn-primary"
                      onClick={() => openReceiveModal(item.id)}
                    >
                      Receive
                    </button>
                    <button 
                      class="btn btn-sm btn-secondary"
                      onClick={() => openTransferModal(item.id)}
                    >
                      Transfer
                    </button>
                    <button 
                      class="btn btn-sm"
                      onClick={() => navigateToItemDetails(item.id)}
                    >
                      Details
                    </button>
                  </div>
                </td>
              </tr>
            );
          })}
        </tbody>
      </table>
    </div>
  );
}
```

#### ReceiveInventoryForm

```tsx
export default function ReceiveInventoryForm() {
  const selectedItems = useSignal<ReceiveItem[]>([]);
  const purchaseOrderId = useSignal<string>("");
  const notes = useSignal<string>("");
  
  const handleAddItem = () => {
    // Logic to add an item to the receiving list
  };
  
  const handleRemoveItem = (index: number) => {
    // Logic to remove an item from the receiving list
  };
  
  const handleSubmit = async () => {
    isLoading.value = true;
    error.value = null;
    
    try {
      await commandBus.dispatch(receiveInventory({
        purchaseOrderId: purchaseOrderId.value || undefined,
        items: selectedItems.value.map(item => ({
          itemId: item.itemId,
          quantity: item.quantity,
          locationId: item.locationId,
          unitCost: item.unitCost,
          batchNumber: item.batchNumber,
          expirationDate: item.expirationDate
        })),
        notes: notes.value
      }));
      
      // Success handling
      alert("Inventory received successfully");
      
      // Reset form and refresh inventory
      selectedItems.value = [];
      purchaseOrderId.value = "";
      notes.value = "";
      loadInventoryItems();
      
    } catch (e) {
      error.value = e.message;
    } finally {
      isLoading.value = false;
    }
  };
  
  return (
    <div class="flex flex-col gap-4">
      <h2 class="text-2xl font-bold">Receive Inventory</h2>
      
      {error.value && (
        <div class="alert alert-error">
          <span>{error.value}</span>
        </div>
      )}
      
      {/* Form fields and item selection */}
      
      <div class="overflow-x-auto">
        <table class="table w-full">
          <thead>
            <tr>
              <th>Item</th>
              <th>Quantity</th>
              <th>Unit Cost</th>
              <th>Location</th>
              <th>Batch #</th>
              <th>Expiration</th>
              <th>Actions</th>
            </tr>
          </thead>
          <tbody>
            {selectedItems.value.map((item, index) => (
              <tr key={index}>
                <td>{item.itemName}</td>
                <td>
                  <input 
                    type="number" 
                    class="input input-bordered w-24" 
                    value={item.quantity} 
                    onChange={e => updateItemQuantity(index, e.target.value)}
                  />
                </td>
                <td>
                  <input 
                    type="number" 
                    class="input input-bordered w-24" 
                    value={item.unitCost} 
                    onChange={e => updateItemCost(index, e.target.value)}
                  />
                </td>
                <td>
                  <select 
                    class="select select-bordered" 
                    value={item.locationId}
                    onChange={e => updateItemLocation(index, e.target.value)}
                  >
                    {locations.value.map(loc => (
                      <option key={loc.id} value={loc.id}>{loc.name}</option>
                    ))}
                  </select>
                </td>
                <td>
                  <input 
                    type="text" 
                    class="input input-bordered w-32" 
                    value={item.batchNumber || ''} 
                    onChange={e => updateItemBatch(index, e.target.value)}
                  />
                </td>
                <td>
                  <input 
                    type="date" 
                    class="input input-bordered" 
                    value={item.expirationDate || ''} 
                    onChange={e => updateItemExpiration(index, e.target.value)}
                  />
                </td>
                <td>
                  <button 
                    class="btn btn-sm btn-error"
                    onClick={() => handleRemoveItem(index)}
                  >
                    Remove
                  </button>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>
      
      <div class="flex justify-end gap-2">
        <button 
          class="btn btn-primary"
          onClick={handleSubmit}
          disabled={selectedItems.value.length === 0 || isLoading.value}
        >
          {isLoading.value ? 'Processing...' : 'Submit Receiving'}
        </button>
      </div>
    </div>
  );
}
```

## Security Considerations

The inventory management system implements several security measures:

1. **Authentication and Authorization**
   - Role-based access control for inventory functions
   - Transaction logging for audit trails
   - Approval workflows for sensitive operations

2. **Data Validation**
   - Input validation for all inventory transactions
   - Business rule enforcement (e.g., can't consume more than available)
   - Consistency checks for inventory data

3. **Financial Controls**
   - Separation of duties for inventory operations
   - Approval workflows for high-value adjustments
   - Cost change monitoring and alerts

## Performance Considerations

To ensure optimal performance:

1. **Optimized Queries**
   - Efficient database indexing for inventory queries
   - Pagination for large inventory sets
   - Cached read models for frequently accessed data

2. **Background Processing**
   - Asynchronous processing for non-critical operations
   - Scheduled tasks for recurring inventory operations
   - Batch processing for bulk updates

3. **UI Optimizations**
   - Lazy loading of inventory data
   - Virtual scrolling for large inventory lists
   - Progressive loading of inventory details

## License

This project is licensed under the MIT License - see the LICENSE file for details.
