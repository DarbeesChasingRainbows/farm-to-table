using RestaurantManagement.InventoryService.Domain.Entities;

namespace RestaurantManagement.InventoryService.Infrastructure.Data.Repositories.Interfaces
{
    public interface IStockLevelRepository : IRepository<StockLevel>
    {
        Task<StockLevel?> GetByItemAndLocationAsync(string itemId, string locationId);
        Task<IReadOnlyList<StockLevel>> GetByItemIdAsync(string itemId);
        Task<IReadOnlyList<StockLevel>> GetByLocationIdAsync(string locationId);
        Task<IReadOnlyList<StockLevel>> GetLowStockLevelsAsync();
        Task<IReadOnlyList<StockLevel>> GetOutOfStockItemsAsync();
        Task<double> GetTotalQuantityForItemAsync(string itemId);
        Task<double> GetAvailableQuantityForItemAsync(string itemId, string locationId);
        Task UpdateStockQuantityAsync(string itemId, string locationId, double newQuantity);
        Task ReserveStockAsync(string itemId, string locationId, double quantity);
        Task ReleaseStockAsync(string itemId, string locationId, double quantity);
    }
}
