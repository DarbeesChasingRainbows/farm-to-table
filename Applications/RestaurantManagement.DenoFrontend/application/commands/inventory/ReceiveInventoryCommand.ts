// application/commands/inventory/ReceiveInventoryCommand.ts
import { Command } from "../CommandBus.ts";

export interface ReceiveInventoryPayload {
  purchaseOrderId?: string;
  items: Array<{
    itemId: string;
    quantity: number;
    locationId: string;
    unitCost?: number;
  }>;
}

export function receiveInventory(payload: ReceiveInventoryPayload): Command {
  return {
    type: "RECEIVE_INVENTORY",
    payload
  };
}

// Register handler in your app initialization
// commandBus.register("RECEIVE_INVENTORY", async (command) => {
//   const { payload } = command;
//   const response = await fetch('/api/inventory/transactions/receive', {
//     method: 'POST',
//     headers: { 'Content-Type': 'application/json' },
//     body: JSON.stringify(payload)
//   });
//   
//   if (!response.ok) {
//     throw new Error('Failed to receive inventory');
//   }
//   
//   // Refresh inventory data
//   await loadInventoryItems(selectedLocationId.value!);
// });