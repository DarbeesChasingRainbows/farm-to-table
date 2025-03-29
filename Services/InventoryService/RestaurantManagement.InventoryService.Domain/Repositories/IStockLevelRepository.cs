using RestaurantManagement.InventoryService.Domain.Entities;

namespace RestaurantManagement.InventoryService.Domain.Repositories;

public interface IStockLevelRepository
{
    Task<StockLevel> GetByItemAndLocationAsync(string itemId, string locationId);
    Task<IEnumerable<StockLevel>> GetByItemAsync(string itemId);
    Task<IEnumerable<StockLevel>> GetByLocationAsync(string locationId);
    Task<IEnumerable<StockLevel>> GetLowStockAsync(string locationId = null);
    Task AddAsync(StockLevel stockLevel);
    Task UpdateAsync(StockLevel stockLevel);
}
