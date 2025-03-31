using Microsoft.EntityFrameworkCore;
using RestaurantManagement.InventoryService.Domain.Entities;
using RestaurantManagement.InventoryService.Infrastructure.Data.Context;
using RestaurantManagement.InventoryService.Infrastructure.Data.Repositories.Interfaces;

namespace RestaurantManagement.InventoryService.Infrastructure.Data.Repositories
{
    public class CountSheetRepository : Repository<CountSheet>, ICountSheetRepository
    {
        public CountSheetRepository(InventoryDbContext context)
            : base(context) { }

        public async Task<IReadOnlyList<CountSheet>> GetByLocationIdAsync(string locationId)
        {
            return await _dbSet
                .Where(c => c.LocationId == locationId)
                .OrderByDescending(c => c.CountDate)
                .ToListAsync();
        }

        public async Task<IReadOnlyList<CountSheet>> GetByStatusAsync(string status)
        {
            return await _dbSet
                .Where(c => c.Status == status)
                .OrderByDescending(c => c.CountDate)
                .ToListAsync();
        }

        public async Task<IReadOnlyList<CountSheet>> GetByDateRangeAsync(
            DateTime startDate,
            DateTime endDate
        )
        {
            return await _dbSet
                .Where(c => c.CountDate >= startDate && c.CountDate <= endDate)
                .OrderByDescending(c => c.CountDate)
                .ToListAsync();
        }

        public async Task<IReadOnlyList<CountSheet>> GetByRequestedByAsync(string userId)
        {
            return await _dbSet
                .Where(c => c.RequestedBy == userId)
                .OrderByDescending(c => c.CountDate)
                .ToListAsync();
        }

        public async Task<IReadOnlyList<CountSheet>> GetByCountedByAsync(string userId)
        {
            return await _dbSet
                .Where(c => c.CountedBy == userId)
                .OrderByDescending(c => c.CountDate)
                .ToListAsync();
        }

        public async Task<IReadOnlyList<CountSheet>> GetByApprovedByAsync(string userId)
        {
            return await _dbSet
                .Where(c => c.ApprovedBy == userId)
                .OrderByDescending(c => c.CountDate)
                .ToListAsync();
        }

        public async Task<CountSheet?> GetWithItemsAsync(string countSheetId)
        {
            return await _dbSet
                .Include(c => c.Items)
                .ThenInclude(i => i.BatchCounts)
                .FirstOrDefaultAsync(c => c.Id == countSheetId);
        }

        public async Task<CountSheetItem?> GetCountSheetItemAsync(
            string countSheetId,
            string itemId
        )
        {
            var countSheet = await _dbSet
                .Include(c => c.Items.Where(i => i.ItemId == itemId))
                .ThenInclude(i => i.BatchCounts)
                .FirstOrDefaultAsync(c => c.Id == countSheetId);

            return countSheet?.Items.FirstOrDefault(i => i.ItemId == itemId);
        }

        public async Task AddCategoryAsync(string countSheetId, string category)
        {
            var countSheet = await _dbSet.FindAsync(countSheetId);

            if (countSheet == null)
            {
                throw new ArgumentException($"Count sheet with ID {countSheetId} not found.");
            }

            countSheet.AddCategory(category);
        }

        public async Task AddItemAsync(
            string countSheetId,
            string itemId,
            string itemName,
            string sku,
            string category,
            string unitOfMeasure,
            double systemQuantity
        )
        {
            var countSheet = await _dbSet
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.Id == countSheetId);

            if (countSheet == null)
            {
                throw new ArgumentException($"Count sheet with ID {countSheetId} not found.");
            }

            // Check if the item already exists
            if (countSheet.Items.Any(i => i.ItemId == itemId))
            {
                throw new ArgumentException(
                    $"Item with ID {itemId} already exists in the count sheet."
                );
            }

            // Add the item to the count sheet
            var countSheetItemId = Guid.NewGuid().ToString();
            countSheet.AddItem(itemId, itemName, sku, category, unitOfMeasure, systemQuantity);
        }

        public async Task RecordCountAsync(
            string countSheetId,
            string itemId,
            double countedQuantity,
            string? batchId = null
        )
        {
            var countSheet = await _dbSet
                .Include(c => c.Items.Where(i => i.ItemId == itemId))
                .FirstOrDefaultAsync(c => c.Id == countSheetId);

            if (countSheet == null)
            {
                throw new ArgumentException($"Count sheet with ID {countSheetId} not found.");
            }

            var item = countSheet.Items.FirstOrDefault(i => i.ItemId == itemId);
            if (item == null)
            {
                throw new ArgumentException($"Item with ID {itemId} not found in the count sheet.");
            }

            if (batchId == null)
            {
                // Record a count for the entire item
                item.RecordCount(countedQuantity);
            }
            else
            {
                // Record a count for a specific batch
                item.RecordBatchCount(batchId, countedQuantity);
            }
        }

        public async Task RecordBatchCountAsync(
            string countSheetId,
            string itemId,
            string batchId,
            double countedQuantity
        )
        {
            var countSheet = await _dbSet
                .Include(c => c.Items.Where(i => i.ItemId == itemId))
                .ThenInclude(i => i.BatchCounts)
                .FirstOrDefaultAsync(c => c.Id == countSheetId);

            if (countSheet == null)
            {
                throw new ArgumentException($"Count sheet with ID {countSheetId} not found.");
            }

            var item = countSheet.Items.FirstOrDefault(i => i.ItemId == itemId);
            if (item == null)
            {
                throw new ArgumentException($"Item with ID {itemId} not found in the count sheet.");
            }

            // Check if the batch exists in the item
            var batchCount = item.BatchCounts.FirstOrDefault(bc => bc.BatchId == batchId);
            if (batchCount == null)
            {
                throw new ArgumentException(
                    $"Batch with ID {batchId} not found in the count sheet item."
                );
            }

            // Record the batch count
            batchCount.RecordCount(countedQuantity);
        }

        public async Task ApproveVarianceAsync(
            string countSheetId,
            string itemId,
            string reasonCode
        )
        {
            var countSheet = await _dbSet
                .Include(c => c.Items.Where(i => i.ItemId == itemId))
                .FirstOrDefaultAsync(c => c.Id == countSheetId);

            if (countSheet == null)
            {
                throw new ArgumentException($"Count sheet with ID {countSheetId} not found.");
            }

            var item = countSheet.Items.FirstOrDefault(i => i.ItemId == itemId);
            if (item == null)
            {
                throw new ArgumentException($"Item with ID {itemId} not found in the count sheet.");
            }

            // Approve the variance
            item.ApproveVariance(reasonCode);
        }

        public async Task CompleteCountSheetAsync(string countSheetId, DateTime completedDate)
        {
            var countSheet = await _dbSet
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.Id == countSheetId);

            if (countSheet == null)
            {
                throw new ArgumentException($"Count sheet with ID {countSheetId} not found.");
            }

            // Check if all items have been counted
            var uncountedItems = countSheet.Items.Where(i => !i.HasBeenCounted).ToList();
            if (uncountedItems.Any())
            {
                throw new ArgumentException(
                    $"Cannot complete count sheet. There are {uncountedItems.Count} items that have not been counted."
                );
            }

            // Complete the count sheet
            countSheet.CompleteCounting(completedDate);
        }

        public async Task ApproveCountSheetAsync(
            string countSheetId,
            string approvedBy,
            DateTime approvalDate
        )
        {
            var countSheet = await _dbSet
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.Id == countSheetId);

            if (countSheet == null)
            {
                throw new ArgumentException($"Count sheet with ID {countSheetId} not found.");
            }

            // Approve the count sheet
            countSheet.ApproveVariances(approvedBy, approvalDate);
        }

        public override async Task<CountSheet?> GetByIdAsync(string id)
        {
            return await _dbSet
                .Include(c => c.Items)
                .ThenInclude(i => i.BatchCounts)
                .FirstOrDefaultAsync(c => c.Id == id);
        }
    }
}
