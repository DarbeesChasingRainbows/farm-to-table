using RestaurantManagement.InventoryService.Domain.Entities;

namespace RestaurantManagement.InventoryService.Infrastructure.Data.Repositories.Interfaces
{
    public interface IBatchRepository : IRepository<Batch>
    {
        Task<Batch?> GetByBatchNumberAsync(string itemId, string batchNumber);
        Task<IReadOnlyList<Batch>> GetByItemIdAsync(string itemId);
        Task<IReadOnlyList<Batch>> GetByLocationIdAsync(string locationId);
        Task<IReadOnlyList<Batch>> GetByVendorIdAsync(string vendorId);
        Task<IReadOnlyList<Batch>> GetExpiringBatchesAsync(int daysThreshold);
        Task<IReadOnlyList<Batch>> GetExpiredBatchesAsync();
        Task<IReadOnlyList<Batch>> GetActiveBatchesAsync(string itemId);
        Task UpdateRemainingQuantityAsync(string batchId, double newQuantity);
        Task TransferBatchAsync(string batchId, string newLocationId);
        Task<bool> IsBatchNumberUniqueAsync(
            string itemId,
            string batchNumber,
            string? excludeBatchId = null
        );
        Task<double> GetTotalQuantityForItemAsync(string itemId);
    }
}
