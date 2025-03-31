using RestaurantManagement.InventoryService.Domain.Entities;

namespace RestaurantManagement.InventoryService.Infrastructure.Data.Repositories.Interfaces
{
    public interface IInventoryItemRepository : IRepository<InventoryItem>
    {
        Task<InventoryItem?> GetBySkuAsync(string sku);
        Task<IReadOnlyList<InventoryItem>> GetByCategoryAsync(string category);
        Task<IReadOnlyList<InventoryItem>> GetByVendorIdAsync(string vendorId);
        Task<IReadOnlyList<InventoryItem>> GetLowStockItemsAsync();
        Task<IReadOnlyList<InventoryItem>> SearchItemsAsync(string searchTerm);
        Task<bool> IsSkuUniqueAsync(string sku, string? excludeItemId = null);
        Task<IReadOnlyList<InventoryItem>> GetItemsWithAlternativesAsync();
        Task AddAlternativeItemAsync(string itemId, string alternativeItemId);
        Task RemoveAlternativeItemAsync(string itemId, string alternativeItemId);
        Task<decimal> GetInventoryValueAsync();
    }
}
