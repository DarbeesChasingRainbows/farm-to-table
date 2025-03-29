using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RestaurantManagement.InventoryService.Domain.Entities;
using RestaurantManagement.InventoryService.Domain.Enums;
using RestaurantManagement.InventoryService.Domain.Repositories;

namespace RestaurantManagement.InventoryService.Domain.Services
{
    public class BatchSelectionService
    {
        private readonly IBatchRepository _batchRepository;
        private readonly IInventoryItemRepository _inventoryItemRepository;

        public BatchSelectionService(
            IBatchRepository batchRepository,
            IInventoryItemRepository inventoryItemRepository)
        {
            _batchRepository = batchRepository;
            _inventoryItemRepository = inventoryItemRepository;
        }

        public async Task<List<(Batch Batch, double Quantity)>> SelectBatchesForConsumptionAsync(
            string itemId, 
            double totalQuantity, 
            string locationId,
            List<string> specificBatchIds = null)
        {
            var item = await _inventoryItemRepository.GetByIdAsync(itemId);
            if (item == null)
                return new List<(Batch, double)>();

            if (!item.TrackExpiration)
                return new List<(Batch, double)>();

            List<Batch> batches;
            if (specificBatchIds != null && specificBatchIds.Any())
            {
                batches = await _batchRepository.GetByIdsAsync(specificBatchIds);
                batches = batches.Where(b => 
                    b.InventoryItemId == itemId && 
                    b.LocationId == locationId && 
                    b.RemainingQuantity > 0).ToList();
            }
            else
            {
                batches = await _batchRepository.GetActiveByItemAndLocationAsync(itemId, locationId);
                batches = batches.Where(b => b.RemainingQuantity > 0).ToList();

                // Sort based on costing method
                switch (item.CostingMethod)
                {
                    case "FIFO":
                        batches = batches.OrderBy(b => b.ReceivedDate).ToList();
                        break;
                    case "LIFO":
                        batches = batches.OrderByDescending(b => b.ReceivedDate).ToList();
                        break;
                    case "FEFO":
                        batches = batches.OrderBy(b => b.ExpirationDate).ToList();
                        break;
                    default:
                        batches = batches.OrderBy(b => b.ExpirationDate).ToList(); // Default to FEFO
                        break;
                }
            }

            var result = new List<(Batch, double)>();
            double remainingToConsume = totalQuantity;

            foreach (var batch in batches)
            {
                if (remainingToConsume <= 0)
                    break;

                double quantityFromBatch = Math.Min(batch.RemainingQuantity, remainingToConsume);
                result.Add((batch, quantityFromBatch));
                remainingToConsume -= quantityFromBatch;
            }

            return result;
        }

        public async Task<(bool Success, List<(Batch Batch, double Quantity)> SelectedBatches, double UnfulfilledQuantity)> 
            TrySelectBatchesAsync(string itemId, double totalQuantity, string locationId)
        {
            var selectedBatches = await SelectBatchesForConsumptionAsync(itemId, totalQuantity, locationId);
            
            double totalSelectedQuantity = selectedBatches.Sum(b => b.Quantity);
            double unfulfilledQuantity = totalQuantity - totalSelectedQuantity;
            
            bool success = unfulfilledQuantity <= 0;
            
            return (success, selectedBatches, unfulfilledQuantity);
        }

        public async Task<double> GetWeightedAverageCostAsync(string itemId)
        {
            var batches = await _batchRepository.GetByItemIdAsync(itemId);
            
            if (!batches.Any())
                return 0;
                
            double totalQuantity = batches.Sum(b => b.RemainingQuantity);
            if (totalQuantity <= 0)
                return 0;
                
            double totalValue = batches.Sum(b => (double)b.UnitCost * b.RemainingQuantity);
            
            return totalValue / totalQuantity;
        }

        public async Task<List<Batch>> FindExpiringBatchesAsync(int daysThreshold, string locationId = null)
        {
            DateTime thresholdDate = DateTime.UtcNow.AddDays(daysThreshold);
            
            var batches = await _batchRepository.GetByExpirationDateAsync(thresholdDate, locationId);
            
            return batches.Where(b => b.RemainingQuantity > 0).ToList();
        }

        public async Task<Dictionary<string, List<Batch>>> GetBatchesByExpirationStatusAsync(
            string locationId = null, 
            bool includeExpired = false)
        {
            var result = new Dictionary<string, List<Batch>>
            {
                { "expiring", new List<Batch>() },
                { "expired", new List<Batch>() },
                { "active", new List<Batch>() }
            };
            
            var batches = locationId == null ? 
                await _batchRepository.GetAllAsync() : 
                await _batchRepository.GetByLocationAsync(locationId);
                
            batches = batches.Where(b => b.RemainingQuantity > 0).ToList();
            
            DateTime now = DateTime.UtcNow;
            DateTime expiringThreshold = now.AddDays(7); // Default 7 days for expiring soon
            
            foreach (var batch in batches)
            {
                if (batch.ExpirationDate <= now)
                {
                    if (includeExpired)
                        result["expired"].Add(batch);
                }
                else if (batch.ExpirationDate <= expiringThreshold)
                {
                    result["expiring"].Add(batch);
                }
                else
                {
                    result["active"].Add(batch);
                }
            }
            
            return result;
        }
    }
}
