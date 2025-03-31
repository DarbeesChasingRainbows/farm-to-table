using Microsoft.EntityFrameworkCore;
using RestaurantManagement.InventoryService.Domain.Entities;
using RestaurantManagement.InventoryService.Infrastructure.Data.Context;
using RestaurantManagement.InventoryService.Infrastructure.Data.Repositories.Interfaces;

namespace RestaurantManagement.InventoryService.Infrastructure.Data.Repositories
{
    public class StockLevelRepository : Repository<StockLevel>, IStockLevelRepository
    {
        public StockLevelRepository(InventoryDbContext context)
            : base(context) { }

        public async Task<StockLevel?> GetByItemAndLocationAsync(string itemId, string locationId)
        {
            return await _dbSet
                .Include(s => s.InventoryItem)
                .FirstOrDefaultAsync(s =>
                    s.InventoryItemId == itemId && s.LocationId == locationId
                );
        }

        public async Task<IReadOnlyList<StockLevel>> GetByItemIdAsync(string itemId)
        {
            return await _dbSet
                .Include(s => s.InventoryItem)
                .Where(s => s.InventoryItemId == itemId)
                .ToListAsync();
        }

        public async Task<IReadOnlyList<StockLevel>> GetByLocationIdAsync(string locationId)
        {
            return await _dbSet
                .Include(s => s.InventoryItem)
                .Where(s => s.LocationId == locationId)
                .ToListAsync();
        }

        public async Task<IReadOnlyList<StockLevel>> GetLowStockLevelsAsync()
        {
            return await _dbSet
                .Include(s => s.InventoryItem)
                .Where(s => s.CurrentQuantity <= s.InventoryItem.ReorderThreshold)
                .ToListAsync();
        }

        public async Task<IReadOnlyList<StockLevel>> GetOutOfStockItemsAsync()
        {
            return await _dbSet
                .Include(s => s.InventoryItem)
                .Where(s => s.CurrentQuantity <= 0)
                .ToListAsync();
        }

        public async Task<double> GetTotalQuantityForItemAsync(string itemId)
        {
            return await _dbSet
                .Where(s => s.InventoryItemId == itemId)
                .SumAsync(s => s.CurrentQuantity);
        }

        public async Task<double> GetAvailableQuantityForItemAsync(string itemId, string locationId)
        {
            var stockLevel = await _dbSet.FirstOrDefaultAsync(s =>
                s.InventoryItemId == itemId && s.LocationId == locationId
            );

            return stockLevel?.AvailableQuantity ?? 0;
        }

        public async Task UpdateStockQuantityAsync(
            string itemId,
            string locationId,
            double newQuantity
        )
        {
            var stockLevel = await _dbSet.FirstOrDefaultAsync(s =>
                s.InventoryItemId == itemId && s.LocationId == locationId
            );

            if (stockLevel == null)
            {
                // Create a new stock level if it doesn't exist
                stockLevel = new StockLevel(Guid.NewGuid().ToString(), itemId, locationId);
                await _dbSet.AddAsync(stockLevel);
            }

            stockLevel.UpdateQuantity(newQuantity);
            stockLevel.LastUpdated = DateTime.UtcNow;
        }

        public async Task ReserveStockAsync(string itemId, string locationId, double quantity)
        {
            var stockLevel = await _dbSet.FirstOrDefaultAsync(s =>
                s.InventoryItemId == itemId && s.LocationId == locationId
            );

            if (stockLevel == null)
            {
                throw new ArgumentException(
                    $"Stock level for item {itemId} at location {locationId} not found."
                );
            }

            stockLevel.Reserve(quantity);
            stockLevel.LastUpdated = DateTime.UtcNow;
        }

        public async Task ReleaseStockAsync(string itemId, string locationId, double quantity)
        {
            var stockLevel = await _dbSet.FirstOrDefaultAsync(s =>
                s.InventoryItemId == itemId && s.LocationId == locationId
            );

            if (stockLevel == null)
            {
                throw new ArgumentException(
                    $"Stock level for item {itemId} at location {locationId} not found."
                );
            }

            stockLevel.Release(quantity);
            stockLevel.LastUpdated = DateTime.UtcNow;
        }

        public override async Task<StockLevel?> GetByIdAsync(string id)
        {
            return await _dbSet.Include(s => s.InventoryItem).FirstOrDefaultAsync(s => s.Id == id);
        }
    }
}
