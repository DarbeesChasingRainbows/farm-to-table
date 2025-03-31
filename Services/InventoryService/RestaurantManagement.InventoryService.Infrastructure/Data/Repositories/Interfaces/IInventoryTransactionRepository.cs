using RestaurantManagement.InventoryService.Domain.Entities;

namespace RestaurantManagement.InventoryService.Infrastructure.Data.Repositories.Interfaces
{
    public interface IInventoryTransactionRepository : IRepository<InventoryTransaction>
    {
        Task<IReadOnlyList<InventoryTransaction>> GetByDateRangeAsync(
            DateTime startDate,
            DateTime endDate
        );
        Task<IReadOnlyList<InventoryTransaction>> GetByTypeAsync(string type);
        Task<IReadOnlyList<InventoryTransaction>> GetByItemIdAsync(string itemId);
        Task<IReadOnlyList<InventoryTransaction>> GetByLocationIdAsync(string locationId);
        Task<IReadOnlyList<InventoryTransaction>> GetByReferenceAsync(
            string referenceNumber,
            string referenceType
        );
        Task<IReadOnlyList<InventoryTransaction>> GetByUserIdAsync(string userId);
        Task<InventoryTransaction?> GetWithItemsAsync(string transactionId);
        Task<IReadOnlyList<InventoryTransaction>> GetRecentTransactionsAsync(int count);
        Task AddTransactionItemAsync(
            string transactionId,
            string itemId,
            double quantity,
            string locationId,
            string? batchId = null,
            decimal? unitCost = null
        );
    }
}
