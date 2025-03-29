using RestaurantManagement.InventoryService.Domain.Entities;

namespace RestaurantManagement.InventoryService.Domain.Repositories;

public interface IInventoryItemRepository
{
    Task<InventoryItem> GetByIdAsync(string id);
    Task<InventoryItem> GetBySkuAsync(string sku);
    Task<IEnumerable<InventoryItem>> GetAllAsync();
    Task<IEnumerable<InventoryItem>> GetByCategoryAsync(string category);
    Task<IEnumerable<InventoryItem>> SearchAsync(string searchTerm);
    Task AddAsync(InventoryItem item);
    Task UpdateAsync(InventoryItem item);
    Task<bool> ExistsAsync(string id);
    Task<bool> SkuExistsAsync(string sku);
}
