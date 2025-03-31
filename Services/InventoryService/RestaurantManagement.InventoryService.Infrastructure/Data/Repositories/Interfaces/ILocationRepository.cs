using RestaurantManagement.InventoryService.Domain.Entities;

namespace RestaurantManagement.InventoryService.Infrastructure.Data.Repositories.Interfaces
{
    public interface ILocationRepository : IRepository<Location>
    {
        Task<Location?> GetByNameAsync(string name);
        Task<IReadOnlyList<Location>> GetByTypeAsync(string type);
        Task<IReadOnlyList<Location>> GetActiveLocationsAsync();
        Task<bool> IsNameUniqueAsync(string name, string? excludeLocationId = null);
        Task<IReadOnlyList<Location>> GetLocationsWithItemsAsync(string itemId);
        Task<IReadOnlyList<Location>> SearchLocationsAsync(string searchTerm);
        Task<IReadOnlyList<Location>> GetLocationsWithStorageConditionsAsync(
            double? minTemperature,
            double? maxTemperature
        );
    }
}
