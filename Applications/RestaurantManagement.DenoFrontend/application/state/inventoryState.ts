// application/state/inventoryState.ts
import { signal, computed } from "@preact/signals";
import type { InventoryItem, StockLevel } from "../../domain/models/inventory.ts";

export const inventoryItems = signal<InventoryItem[]>([]);
export const selectedLocationId = signal<string | null>(null);
export const isLoading = signal<boolean>(false);
export const error = signal<string | null>(null);

// Computed values
export const itemsByCategory = computed(() => {
  const result = new Map<string, InventoryItem[]>();
  
  for (const item of inventoryItems.value) {
    if (!result.has(item.category)) {
      result.set(item.category, []);
    }
    result.get(item.category)!.push(item);
  }
  
  return result;
});

export const lowStockItems = computed(() => {
  return inventoryItems.value.filter(item => {
    const stockLevel = item.stockLevels.find(sl => 
      sl.locationId === selectedLocationId.value
    );
    return stockLevel && stockLevel.availableQuantity < item.reorderThreshold;
  });
});

// Actions
export async function loadInventoryItems(locationId: string) {
  isLoading.value = true;
  error.value = null;
  
  try {
    selectedLocationId.value = locationId;
    // This would be a call to your API gateway
    const response = await fetch(`/api/inventory/items?locationId=${locationId}`);
    
    if (!response.ok) {
      throw new Error('Failed to load inventory items');
    }
    
    const data = await response.json();
    inventoryItems.value = data;
  } catch (e) {
    error.value = e.message;
  } finally {
    isLoading.value = false;
  }
}