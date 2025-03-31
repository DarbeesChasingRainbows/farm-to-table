using Microsoft.EntityFrameworkCore;
using RestaurantManagement.InventoryService.Domain.Entities;
using RestaurantManagement.InventoryService.Infrastructure.Data.Context;
using RestaurantManagement.InventoryService.Infrastructure.Data.Repositories.Interfaces;

namespace RestaurantManagement.InventoryService.Infrastructure.Data.Repositories
{
    public class InventoryTransactionRepository
        : Repository<InventoryTransaction>,
            IInventoryTransactionRepository
    {
        public InventoryTransactionRepository(InventoryDbContext context)
            : base(context) { }

        public async Task<IReadOnlyList<InventoryTransaction>> GetByDateRangeAsync(
            DateTime startDate,
            DateTime endDate
        )
        {
            return await _dbSet
                .Where(t => t.TransactionDate >= startDate && t.TransactionDate <= endDate)
                .OrderByDescending(t => t.TransactionDate)
                .ToListAsync();
        }

        public async Task<IReadOnlyList<InventoryTransaction>> GetByTypeAsync(string type)
        {
            return await _dbSet
                .Where(t => t.Type == type)
                .OrderByDescending(t => t.TransactionDate)
                .ToListAsync();
        }

        public async Task<IReadOnlyList<InventoryTransaction>> GetByItemIdAsync(string itemId)
        {
            // This is a bit more complex as we need to find transactions that have a specific item
            return await _dbSet
                .Where(t => t.Items.Any(i => i.InventoryItemId == itemId))
                .OrderByDescending(t => t.TransactionDate)
                .ToListAsync();
        }

        public async Task<IReadOnlyList<InventoryTransaction>> GetByLocationIdAsync(
            string locationId
        )
        {
            return await _dbSet
                .Where(t => t.LocationId == locationId || t.DestinationLocationId == locationId)
                .OrderByDescending(t => t.TransactionDate)
                .ToListAsync();
        }

        public async Task<IReadOnlyList<InventoryTransaction>> GetByReferenceAsync(
            string referenceNumber,
            string referenceType
        )
        {
            return await _dbSet
                .Where(t =>
                    t.ReferenceNumber == referenceNumber && t.ReferenceType == referenceType
                )
                .OrderByDescending(t => t.TransactionDate)
                .ToListAsync();
        }

        public async Task<IReadOnlyList<InventoryTransaction>> GetByUserIdAsync(string userId)
        {
            return await _dbSet
                .Where(t => t.UserId == userId)
                .OrderByDescending(t => t.TransactionDate)
                .ToListAsync();
        }

        public async Task<InventoryTransaction?> GetWithItemsAsync(string transactionId)
        {
            return await _dbSet
                .Where(t => t.Id == transactionId)
                .Include(t => t.Items)
                .ThenInclude(i => i.InventoryItem)
                .FirstOrDefaultAsync();
        }

        public async Task<IReadOnlyList<InventoryTransaction>> GetRecentTransactionsAsync(int count)
        {
            return await _dbSet.OrderByDescending(t => t.TransactionDate).Take(count).ToListAsync();
        }

        public async Task AddTransactionItemAsync(
            string transactionId,
            string itemId,
            double quantity,
            string locationId,
            string? batchId = null,
            decimal? unitCost = null
        )
        {
            var transaction = await _dbSet
                .Include(t => t.Items)
                .FirstOrDefaultAsync(t => t.Id == transactionId);

            if (transaction == null)
            {
                throw new ArgumentException($"Transaction with ID {transactionId} not found.");
            }

            // Check if the inventory item exists
            var inventoryItem = await _context.Set<InventoryItem>().FindAsync(itemId);
            if (inventoryItem == null)
            {
                throw new ArgumentException($"Inventory item with ID {itemId} not found.");
            }

            // Create a new transaction item ID
            var transactionItemId = Guid.NewGuid().ToString();

            // Add the item to the transaction
            transaction.AddItem(itemId, quantity, locationId, batchId, unitCost);
        }

        public override async Task<InventoryTransaction?> GetByIdAsync(string id)
        {
            return await _dbSet
                .Include(t => t.Items)
                .ThenInclude(i => i.InventoryItem)
                .FirstOrDefaultAsync(t => t.Id == id);
        }
    }
}
