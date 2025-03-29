// islands/admin/InventoryManagement.tsx
import { useSignal, useComputed } from "@preact/signals";
import { inventoryItems, lowStockItems, loadInventoryItems } from "../../application/state/inventoryState.ts";
import { commandBus } from "../../application/commands/CommandBus.ts";
import { receiveInventory } from "../../application/commands/inventory/ReceiveInventoryCommand.ts";

export default function InventoryManagement() {
  const locationId = useSignal("main-location");
  const selectedCategory = useSignal(null);
  
  const filteredItems = useComputed(() => {
    let items = inventoryItems.value;
    
    if (selectedCategory.value) {
      items = items.filter(item => item.category === selectedCategory.value);
    }
    
    return items;
  });
  
  const handleReceiveInventory = async (itemId, quantity) => {
    await commandBus.dispatch(receiveInventory({
      items: [{
        itemId,
        quantity: parseFloat(quantity),
        locationId: locationId.value
      }]
    }));
  };
  
  useEffect(() => {
    loadInventoryItems(locationId.value);
  }, [locationId.value]);

  return (
    <div class="container mx-auto p-4">
      <h1 class="text-2xl font-bold mb-4">Inventory Management</h1>
      
      <div class="mb-4">
        <label class="label">Location</label>
        <select 
          class="select select-bordered w-full max-w-xs"
          value={locationId.value}
          onChange={(e) => locationId.value = e.target.value}
        >
          <option value="main-location">Main Kitchen</option>
          <option value="bar">Bar</option>
          <option value="storage">Storage</option>
        </select>
      </div>
      
      {lowStockItems.value.length > 0 && (
        <div class="alert alert-warning mb-4">
          <span>{lowStockItems.value.length} items are below reorder threshold</span>
        </div>
      )}
      
      <div class="tabs mb-4">
        <button 
          class={`tab tab-bordered ${selectedCategory.value === null ? 'tab-active' : ''}`}
          onClick={() => selectedCategory.value = null}
        >
          All
        </button>
        <button 
          class={`tab tab-bordered ${selectedCategory.value === 'Vegetables' ? 'tab-active' : ''}`}
          onClick={() => selectedCategory.value = 'Vegetables'}
        >
          Vegetables
        </button>
        <button 
          class={`tab tab-bordered ${selectedCategory.value === 'Meat' ? 'tab-active' : ''}`}
          onClick={() => selectedCategory.value = 'Meat'}
        >
          Meat
        </button>
        {/* More categories */}
      </div>
      
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
              <th>Actions</th>
            </tr>
          </thead>
          <tbody>
            {filteredItems.value.map(item => {
              const stockLevel = item.stockLevels.find(sl => sl.locationId === locationId.value);
              return (
                <tr key={item.id}>
                  <td>{item.name}</td>
                  <td>{item.sku}</td>
                  <td>{item.category}</td>
                  <td>{stockLevel?.currentQuantity ?? 0} {item.unitOfMeasure}</td>
                  <td>{stockLevel?.reservedQuantity ?? 0} {item.unitOfMeasure}</td>
                  <td>{stockLevel?.availableQuantity ?? 0} {item.unitOfMeasure}</td>
                  <td>
                    <button 
                      class="btn btn-sm btn-primary mr-2"
                      onClick={() => {
                        const quantity = prompt(`Enter quantity to receive for ${item.name}:`);
                        if (quantity) handleReceiveInventory(item.id, quantity);
                      }}
                    >
                      Receive
                    </button>
                    <a href={`/admin/inventory/items/${item.id}`} class="btn btn-sm btn-ghost">
                      Details
                    </a>
                  </td>
                </tr>
              );
            })}
          </tbody>
        </table>
      </div>
    </div>
  );
}