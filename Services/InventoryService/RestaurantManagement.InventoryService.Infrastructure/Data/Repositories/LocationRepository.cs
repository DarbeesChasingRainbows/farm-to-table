using Microsoft.EntityFrameworkCore;
using RestaurantManagement.InventoryService.Domain.Entities;
using RestaurantManagement.InventoryService.Infrastructure.Data.Context;
using RestaurantManagement.InventoryService.Infrastructure.Data.Repositories.Interfaces;

namespace RestaurantManagement.InventoryService.Infrastructure.Data.Repositories
{
    public class LocationRepository : Repository<Location>, ILocationRepository
    {
        public LocationRepository(InventoryDbContext context)
            : base(context) { }

        public async Task<Location?> GetByNameAsync(string name)
        {
            return await _dbSet.FirstOrDefaultAsync(l => l.Name.ToLower() == name.ToLower());
        }

        public async Task<IReadOnlyList<Location>> GetByTypeAsync(string type)
        {
            return await _dbSet.Where(l => l.Type.ToLower() == type.ToLower()).ToListAsync();
        }

        public async Task<IReadOnlyList<Location>> GetActiveLocationsAsync()
        {
            return await _dbSet.Where(l => l.IsActive).ToListAsync();
        }

        public async Task<bool> IsNameUniqueAsync(string name, string? excludeLocationId = null)
        {
            if (string.IsNullOrEmpty(excludeLocationId))
            {
                return !await _dbSet.AnyAsync(l => l.Name.ToLower() == name.ToLower());
            }
            else
            {
                return !await _dbSet.AnyAsync(l =>
                    l.Name.ToLower() == name.ToLower() && l.Id != excludeLocationId
                );
            }
        }

        public async Task<IReadOnlyList<Location>> GetLocationsWithItemsAsync(string itemId)
        {
            var stockLevels = await _context
                .Set<StockLevel>()
                .Where(sl => sl.InventoryItemId == itemId && sl.CurrentQuantity > 0)
                .Select(sl => sl.LocationId)
                .ToListAsync();

            return await _dbSet.Where(l => stockLevels.Contains(l.Id)).ToListAsync();
        }

        public async Task<IReadOnlyList<Location>> SearchLocationsAsync(string searchTerm)
        {
            searchTerm = searchTerm.ToLower();

            return await _dbSet
                .Where(l =>
                    l.Name.ToLower().Contains(searchTerm)
                    || l.Description != null && l.Description.ToLower().Contains(searchTerm)
                    || l.Type.ToLower().Contains(searchTerm)
                    || l.SpecialRequirements != null
                        && l.SpecialRequirements.ToLower().Contains(searchTerm)
                )
                .ToListAsync();
        }

        public async Task<IReadOnlyList<Location>> GetLocationsWithStorageConditionsAsync(
            double? minTemperature,
            double? maxTemperature
        )
        {
            var query = _dbSet.AsQueryable();

            if (minTemperature.HasValue)
            {
                query = query.Where(l => l.MinTemperature >= minTemperature.Value);
            }

            if (maxTemperature.HasValue)
            {
                query = query.Where(l => l.MaxTemperature <= maxTemperature.Value);
            }

            return await query.ToListAsync();
        }
    }
}
