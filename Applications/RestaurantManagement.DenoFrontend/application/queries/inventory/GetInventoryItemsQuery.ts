// application/queries/inventory/GetInventoryItemsQuery.ts
import { Query } from "../QueryBus.ts";
import type { InventoryItem } from "../../../domain/models/inventory.ts";

export interface GetInventoryItemsParameters {
  locationId: string;
  category?: string;
  search?: string;
}

export function getInventoryItems(parameters: GetInventoryItemsParameters): Query {
  return {
    type: "GET_INVENTORY_ITEMS",
    parameters
  };
}

// Register handler in your app initialization
// queryBus.register("GET_INVENTORY_ITEMS", async (query) => {
//   const { locationId, category, search } = query.parameters;
//   
//   let url = `/api/inventory/items?locationId=${locationId}`;
//   if (category) url += `&category=${category}`;
//   if (search) url += `&search=${search}`;
//   
//   const response = await fetch(url);
//   
//   if (!response.ok) {
//     throw new Error('Failed to get inventory items');
//   }
//   
//   return await response.json();
// });