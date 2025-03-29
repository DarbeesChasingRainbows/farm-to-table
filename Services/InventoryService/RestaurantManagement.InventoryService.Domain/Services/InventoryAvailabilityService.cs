using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RestaurantManagement.InventoryService.Domain.Entities;
using RestaurantManagement.InventoryService.Domain.Repositories;

namespace RestaurantManagement.InventoryService.Domain.Services
{
    public class InventoryAvailabilityService
    {
        private readonly IStockLevelRepository _stockLevelRepository;
        private readonly IInventoryItemRepository _inventoryItemRepository;
        private readonly IBatchRepository _batchRepository;

        public InventoryAvailabilityService(
            IStockLevelRepository stockLevelRepository,
            IInventoryItemRepository inventoryItemRepository,
            IBatchRepository batchRepository)
        {
            _stockLevelRepository = stockLevelRepository;
            _inventoryItemRepository = inventoryItemRepository;
            _batchRepository = batchRepository;
        }

        public async Task<bool> IsItemAvailableAsync(string itemId, double requiredQuantity, string locationId)
        {
            var stockLevel = await _stockLevelRepository.GetByItemAndLocationAsync(itemId, locationId);
            if (stockLevel == null)
                return false;

            return stockLevel.IsAvailable(requiredQuantity);
        }

        public async Task<Dictionary<string, bool>> CheckAvailabilityAsync(
            Dictionary<string, double> itemQuantities, 
            string locationId)
        {
            var result = new Dictionary<string, bool>();

            foreach (var item in itemQuantities)
            {
                result[item.Key] = await IsItemAvailableAsync(item.Key, item.Value, locationId);
            }

            return result;
        }

        public async Task<(bool isAvailable, List<string> unavailableItems)> AreAllItemsAvailableAsync(
            Dictionary<string, double> itemQuantities, 
            string locationId)
        {
            var availability = await CheckAvailabilityAsync(itemQuantities, locationId);
            var unavailableItems = availability.Where(a => !a.Value).Select(a => a.Key).ToList();

            return (unavailableItems.Count == 0, unavailableItems);
        }

        public async Task<Dictionary<string, List<Batch>>> GetAvailableBatchesAsync(
            string itemId, 
            double requiredQuantity, 
            string locationId)
        {
            var stockLevel = await _stockLevelRepository.GetByItemAndLocationAsync(itemId, locationId);
            if (stockLevel == null || !stockLevel.IsAvailable(requiredQuantity))
                return new Dictionary<string, List<Batch>>();

            var item = await _inventoryItemRepository.GetByIdAsync(itemId);
            if (item == null || !item.TrackExpiration)
                return new Dictionary<string, List<Batch>>();

            var batches = await _batchRepository.GetActiveByItemAndLocationAsync(itemId, locationId);
            var result = new Dictionary<string, List<Batch>>
            {
                { locationId, batches.Where(b => b.RemainingQuantity > 0).ToList() }
            };

            return result;
        }

        public async Task<List<StockLevel>> GetLowStockItemsAsync(string locationId = null)
        {
            var lowStockLevels = new List<StockLevel>();
            IEnumerable<StockLevel> stockLevels;

            if (string.IsNullOrEmpty(locationId))
                stockLevels = await _stockLevelRepository.GetAllAsync();
            else
                stockLevels = await _stockLevelRepository.GetByLocationAsync(locationId);

            foreach (var stockLevel in stockLevels)
            {
                var item = await _inventoryItemRepository.GetByIdAsync(stockLevel.InventoryItemId);
                if (item != null && stockLevel.AvailableQuantity < item.ReorderThreshold)
                {
                    lowStockLevels.Add(stockLevel);
                }
            }

            return lowStockLevels;
        }

        public async Task<List<Batch>> GetExpiringBatchesAsync(int daysThreshold, string locationId = null)
        {
            return await _batchRepository.GetExpiringBatchesAsync(daysThreshold, locationId);
        }

        public async Task<Dictionary<string, List<string>>> FindAlternativeItemsAsync(List<string> itemIds)
        {
            var result = new Dictionary<string, List<string>>();

            foreach (var itemId in itemIds)
            {
                var item = await _inventoryItemRepository.GetByIdAsync(itemId);
                if (item != null)
                {
                    result[itemId] = item.AlternativeItems.Select(i => i.Id).ToList();
                }
                else
                {
                    result[itemId] = new List<string>();
                }
            }

            return result;
        }

        public async Task<Dictionary<string, double>> GetAvailableQuantitiesAsync(List<string> itemIds, string locationId)
        {
            var result = new Dictionary<string, double>();

            foreach (var itemId in itemIds)
            {
                var stockLevel = await _stockLevelRepository.GetByItemAndLocationAsync(itemId, locationId);
                result[itemId] = stockLevel?.AvailableQuantity ?? 0;
            }

            return result;
        }

        public async Task<Dictionary<string, List<string>>> FindAlternativeLocationsAsync(
            Dictionary<string, double> itemQuantities, 
            string primaryLocationId,
            List<string> alternativeLocationIds)
        {
            var result = new Dictionary<string, List<string>>();

            foreach (var item in itemQuantities)
            {
                var availableInPrimary = await IsItemAvailableAsync(item.Key, item.Value, primaryLocationId);
                
                if (!availableInPrimary)
                {
                    var alternativeLocations = new List<string>();
                    
                    foreach (var locationId in alternativeLocationIds)
                    {
                        var availableInAlternative = await IsItemAvailableAsync(item.Key, item.Value, locationId);
                        if (availableInAlternative)
                        {
                            alternativeLocations.Add(locationId);
                        }
                    }
                    
                    result[item.Key] = alternativeLocations;
                }
                else
                {
                    result[item.Key] = new List<string> { primaryLocationId };
                }
            }

            return result;
        }
    }
}
