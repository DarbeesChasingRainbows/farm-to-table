using Microsoft.EntityFrameworkCore;
using RestaurantManagement.InventoryService.Domain.Entities;
using RestaurantManagement.InventoryService.Infrastructure.Data.Context;
using RestaurantManagement.InventoryService.Infrastructure.Data.Repositories.Interfaces;

namespace RestaurantManagement.InventoryService.Infrastructure.Data.Repositories
{
    public class BatchRepository : Repository<Batch>, IBatchRepository
    {
        public BatchRepository(InventoryDbContext context)
            : base(context) { }

        public async Task<Batch?> GetByBatchNumberAsync(string itemId, string batchNumber)
        {
            return await _dbSet
                .Include(b => b.InventoryItem)
                .FirstOrDefaultAsync(b =>
                    b.InventoryItemId == itemId && b.BatchNumber == batchNumber
                );
        }

        public async Task<IReadOnlyList<Batch>> GetByItemIdAsync(string itemId)
        {
            return await _dbSet
                .Include(b => b.InventoryItem)
                .Where(b => b.InventoryItemId == itemId)
                .OrderBy(b => b.ExpirationDate)
                .ToListAsync();
        }

        public async Task<IReadOnlyList<Batch>> GetByLocationIdAsync(string locationId)
        {
            return await _dbSet
                .Include(b => b.InventoryItem)
                .Where(b => b.LocationId == locationId)
                .OrderBy(b => b.ExpirationDate)
                .ToListAsync();
        }

        public async Task<IReadOnlyList<Batch>> GetByVendorIdAsync(string vendorId)
        {
            return await _dbSet
                .Include(b => b.InventoryItem)
                .Where(b => b.VendorId == vendorId)
                .OrderBy(b => b.InventoryItemId)
                .ThenBy(b => b.ExpirationDate)
                .ToListAsync();
        }

        public async Task<IReadOnlyList<Batch>> GetExpiringBatchesAsync(int daysThreshold)
        {
            var thresholdDate = DateTime.UtcNow.AddDays(daysThreshold);

            return await _dbSet
                .Include(b => b.InventoryItem)
                .Where(b =>
                    b.ExpirationDate <= thresholdDate
                    && b.ExpirationDate >= DateTime.UtcNow
                    && b.RemainingQuantity > 0
                )
                .OrderBy(b => b.ExpirationDate)
                .ToListAsync();
        }

        public async Task<IReadOnlyList<Batch>> GetExpiredBatchesAsync()
        {
            var currentDate = DateTime.UtcNow;

            return await _dbSet
                .Include(b => b.InventoryItem)
                .Where(b => b.ExpirationDate < currentDate && b.RemainingQuantity > 0)
                .OrderBy(b => b.ExpirationDate)
                .ToListAsync();
        }

        public async Task<IReadOnlyList<Batch>> GetActiveBatchesAsync(string itemId)
        {
            return await _dbSet
                .Include(b => b.InventoryItem)
                .Where(b =>
                    b.InventoryItemId == itemId
                    && b.RemainingQuantity > 0
                    && b.ExpirationDate > DateTime.UtcNow
                )
                .OrderBy(b => b.ExpirationDate)
                .ToListAsync();
        }

        public async Task UpdateRemainingQuantityAsync(string batchId, double newQuantity)
        {
            var batch = await _dbSet.FindAsync(batchId);

            if (batch == null)
            {
                throw new ArgumentException($"Batch with ID {batchId} not found.");
            }

            batch.UpdateRemainingQuantity(newQuantity);
        }

        public async Task TransferBatchAsync(string batchId, string newLocationId)
        {
            var batch = await _dbSet.FindAsync(batchId);

            if (batch == null)
            {
                throw new ArgumentException($"Batch with ID {batchId} not found.");
            }

            batch.Transfer(newLocationId);
        }

        public async Task<bool> IsBatchNumberUniqueAsync(
            string itemId,
            string batchNumber,
            string? excludeBatchId = null
        )
        {
            if (string.IsNullOrEmpty(excludeBatchId))
            {
                return !await _dbSet.AnyAsync(b =>
                    b.InventoryItemId == itemId && b.BatchNumber == batchNumber
                );
            }
            else
            {
                return !await _dbSet.AnyAsync(b =>
                    b.InventoryItemId == itemId
                    && b.BatchNumber == batchNumber
                    && b.Id != excludeBatchId
                );
            }
        }

        public async Task<double> GetTotalQuantityForItemAsync(string itemId)
        {
            return await _dbSet
                .Where(b => b.InventoryItemId == itemId && b.RemainingQuantity > 0)
                .SumAsync(b => b.RemainingQuantity);
        }

        public override async Task<Batch?> GetByIdAsync(string id)
        {
            return await _dbSet.Include(b => b.InventoryItem).FirstOrDefaultAsync(b => b.Id == id);
        }
    }
}
