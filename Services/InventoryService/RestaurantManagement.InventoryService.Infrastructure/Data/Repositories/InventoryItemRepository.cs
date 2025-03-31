using Microsoft.EntityFrameworkCore;
using RestaurantManagement.InventoryService.Domain.Entities;
using RestaurantManagement.InventoryService.Infrastructure.Data.Context;
using RestaurantManagement.InventoryService.Infrastructure.Data.Repositories.Interfaces;

namespace RestaurantManagement.InventoryService.Infrastructure.Data.Repositories
{
    public class InventoryItemRepository : Repository<InventoryItem>, IInventoryItemRepository
    {
        public InventoryItemRepository(InventoryDbContext context)
            : base(context) { }

        public async Task<InventoryItem?> GetBySkuAsync(string sku)
        {
            return await _dbSet
                .Include(i => i.StockLevels)
                .Include(i => i.Batches)
                .FirstOrDefaultAsync(i => i.SKU == sku);
        }

        public async Task<IReadOnlyList<InventoryItem>> GetByCategoryAsync(string category)
        {
            return await _dbSet.Where(i => i.Category == category).ToListAsync();
        }

        public async Task<IReadOnlyList<InventoryItem>> GetByVendorIdAsync(string vendorId)
        {
            // Query for items where the vendor is the default vendor
            var defaultVendorItems = await _dbSet
                .Where(i => i.DefaultVendorId == vendorId)
                .ToListAsync();

            // Query for items from the VendorItem table where this vendor supplies the item
            var vendorItems = await _context
                .Set<VendorItem>()
                .Where(vi => vi.VendorId == vendorId)
                .Select(vi => vi.Item)
                .ToListAsync();

            // Combine the two lists and remove duplicates
            return defaultVendorItems.Union(vendorItems).ToList();
        }

        public async Task<IReadOnlyList<InventoryItem>> GetLowStockItemsAsync()
        {
            return await _dbSet
                .Include(i => i.StockLevels)
                .Where(i => i.StockLevels.Any(sl => sl.CurrentQuantity <= i.ReorderThreshold))
                .ToListAsync();
        }

        public async Task<IReadOnlyList<InventoryItem>> SearchItemsAsync(string searchTerm)
        {
            searchTerm = searchTerm.ToLower();

            return await _dbSet
                .Where(i =>
                    i.Name.ToLower().Contains(searchTerm)
                    || i.Description != null && i.Description.ToLower().Contains(searchTerm)
                    || i.SKU.ToLower().Contains(searchTerm)
                    || i.Category.ToLower().Contains(searchTerm)
                    || (i.Subcategory != null && i.Subcategory.ToLower().Contains(searchTerm))
                )
                .ToListAsync();
        }

        public async Task<bool> IsSkuUniqueAsync(string sku, string? excludeItemId = null)
        {
            if (string.IsNullOrEmpty(excludeItemId))
            {
                return !await _dbSet.AnyAsync(i => i.SKU == sku);
            }
            else
            {
                return !await _dbSet.AnyAsync(i => i.SKU == sku && i.Id != excludeItemId);
            }
        }

        public async Task<IReadOnlyList<InventoryItem>> GetItemsWithAlternativesAsync()
        {
            return await _dbSet
                .Include(i => i.AlternativeItems)
                .Where(i => i.AlternativeItems.Any())
                .ToListAsync();
        }

        public async Task AddAlternativeItemAsync(string itemId, string alternativeItemId)
        {
            var inventoryItem = await _dbSet
                .Include(i => i.AlternativeItems)
                .FirstOrDefaultAsync(i => i.Id == itemId);

            if (inventoryItem == null)
            {
                throw new ArgumentException($"Inventory item with ID {itemId} not found.");
            }

            var alternativeItem = await _dbSet.FindAsync(alternativeItemId);

            if (alternativeItem == null)
            {
                throw new ArgumentException(
                    $"Alternative inventory item with ID {alternativeItemId} not found."
                );
            }

            // Check if the alternative item is already added
            if (inventoryItem.AlternativeItems.Any(ai => ai.Id == alternativeItemId))
            {
                return; // Already exists, nothing to do
            }

            // Add the alternative item
            inventoryItem.AlternativeItems.Add(alternativeItem);
        }

        public async Task RemoveAlternativeItemAsync(string itemId, string alternativeItemId)
        {
            var inventoryItem = await _dbSet
                .Include(i => i.AlternativeItems)
                .FirstOrDefaultAsync(i => i.Id == itemId);

            if (inventoryItem == null)
            {
                throw new ArgumentException($"Inventory item with ID {itemId} not found.");
            }

            var alternativeItem = inventoryItem.AlternativeItems.FirstOrDefault(ai =>
                ai.Id == alternativeItemId
            );

            if (alternativeItem == null)
            {
                return; // Already removed, nothing to do
            }

            // Remove the alternative item
            inventoryItem.AlternativeItems.Remove(alternativeItem);
        }

        public async Task<decimal> GetInventoryValueAsync()
        {
            var items = await _dbSet.Include(i => i.StockLevels).ToListAsync();

            decimal totalValue = 0;

            foreach (var item in items)
            {
                decimal itemCost = item.AverageCost > 0 ? item.AverageCost : item.LastCost;
                double totalQuantity = item.GetTotalStockQuantity();
                totalValue += (decimal)totalQuantity * itemCost;
            }

            return totalValue;
        }

        public override async Task<InventoryItem?> GetByIdAsync(string id)
        {
            return await _dbSet
                .Include(i => i.StockLevels)
                .Include(i => i.Batches)
                .Include(i => i.AlternativeItems)
                .FirstOrDefaultAsync(i => i.Id == id);
        }
    }
}
