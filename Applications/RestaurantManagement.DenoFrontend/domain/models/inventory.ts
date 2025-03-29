// domain/models/inventory.ts
export interface InventoryItem {
    id: string;
    name: string;
    description: string;
    sku: string;
    category: string;
    subcategory: string;
    unitOfMeasure: string;
    reorderThreshold: number;
    minStockLevel: number;
    maxStockLevel: number;
    stockLevels: StockLevel[];
  }
  
  export interface StockLevel {
    itemId: string;
    locationId: string;
    currentQuantity: number;
    reservedQuantity: number;
    availableQuantity: number;
  }