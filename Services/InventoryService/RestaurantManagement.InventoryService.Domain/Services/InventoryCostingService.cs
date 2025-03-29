using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RestaurantManagement.InventoryService.Domain.Entities;
using RestaurantManagement.InventoryService.Domain.Enums;
using RestaurantManagement.InventoryService.Domain.Repositories;
using RestaurantManagement.InventoryService.Domain.ValueObjects;

namespace RestaurantManagement.InventoryService.Domain.Services
{
    public class InventoryCostingService
    {
        private readonly IInventoryItemRepository _inventoryItemRepository;
        private readonly IBatchRepository _batchRepository;
        private readonly IInventoryTransactionRepository _transactionRepository;

        public InventoryCostingService(
            IInventoryItemRepository inventoryItemRepository,
            IBatchRepository batchRepository,
            IInventoryTransactionRepository transactionRepository)
        {
            _inventoryItemRepository = inventoryItemRepository;
            _batchRepository = batchRepository;
            _transactionRepository = transactionRepository;
        }

        public async Task<decimal> CalculateAverageCostAsync(string itemId)
        {
            var batches = await _batchRepository.GetByItemIdAsync(itemId);
            
            if (!batches.Any() || batches.Sum(b => b.RemainingQuantity) <= 0)
                return 0;
                
            decimal totalValue = 0;
            double totalQuantity = 0;
            
            foreach (var batch in batches)
            {
                if (batch.RemainingQuantity > 0)
                {
                    totalValue += batch.UnitCost * (decimal)batch.RemainingQuantity;
                    totalQuantity += batch.RemainingQuantity;
                }
            }
            
            if (totalQuantity <= 0)
                return 0;
                
            return totalValue / (decimal)totalQuantity;
        }

        public async Task<decimal> CalculateLatestCostAsync(string itemId)
        {
            var recentTransactions = await _transactionRepository.GetRecentByItemAsync(itemId, InventoryTransactionType.Received.ToString(), 1);
            
            if (!recentTransactions.Any())
                return 0;
                
            var transaction = recentTransactions.First();
            var transactionItem = transaction.Items.FirstOrDefault(i => i.InventoryItemId == itemId);
            
            return transactionItem?.UnitCost ?? 0;
        }

        public async Task<Dictionary<string, decimal>> CalculateInventoryValueAsync(
            string locationId = null, 
            string categoryId = null, 
            DateTime? asOfDate = null)
        {
            asOfDate = asOfDate ?? DateTime.UtcNow;
            var result = new Dictionary<string, decimal>();
            
            // Get all inventory items filtered by category if specified
            IEnumerable<InventoryItem> items;
            if (!string.IsNullOrEmpty(categoryId))
                items = await _inventoryItemRepository.GetByCategoryAsync(categoryId);
            else
                items = await _inventoryItemRepository.GetAllAsync();
                
            foreach (var item in items)
            {
                decimal itemValue = 0;
                
                if (item.TrackExpiration)
                {
                    // For batch-tracked items, calculate from batches
                    var batches = await _batchRepository.GetByItemIdAsync(item.Id);
                    
                    if (!string.IsNullOrEmpty(locationId))
                        batches = batches.Where(b => b.LocationId == locationId).ToList();
                        
                    foreach (var batch in batches)
                    {
                        // Only include batches that existed as of the specified date
                        if (batch.ReceivedDate <= asOfDate)
                        {
                            // Determine the remaining quantity as of the specified date
                            double quantityAsOfDate = batch.RemainingQuantity;
                            if (asOfDate < DateTime.UtcNow)
                            {
                                // This is a historical calculation, need to reconstruct from transactions
                                var consumptionsAfterDate = await _transactionRepository.GetConsumptionsByBatchAfterDateAsync(
                                    batch.Id, asOfDate.Value);
                                    
                                // Add back any consumption that happened after the as-of date
                                foreach (var consumption in consumptionsAfterDate)
                                {
                                    quantityAsOfDate += consumption.Quantity;
                                }
                            }
                            
                            itemValue += batch.UnitCost * (decimal)quantityAsOfDate;
                        }
                    }
                }
                else
                {
                    // For non-batch items, use average cost and current stock levels
                    foreach (var stockLevel in item.StockLevels)
                    {
                        if (string.IsNullOrEmpty(locationId) || stockLevel.LocationId == locationId)
                        {
                            itemValue += item.AverageCost * (decimal)stockLevel.CurrentQuantity;
                        }
                    }
                }
                
                result[item.Id] = itemValue;
            }
            
            return result;
        }

        public async Task<Dictionary<string, decimal>> CalculateConsumptionCostAsync(
            string itemId, 
            double quantity, 
            List<(string BatchId, double Quantity)> batchAllocations = null)
        {
            var result = new Dictionary<string, decimal>
            {
                { "total", 0 }
            };
            
            var item = await _inventoryItemRepository.GetByIdAsync(itemId);
            if (item == null)
                return result;
                
            if (item.TrackExpiration && batchAllocations != null && batchAllocations.Any())
            {
                // Calculate based on specific batch allocation
                decimal totalCost = 0;
                
                foreach (var allocation in batchAllocations)
                {
                    var batch = await _batchRepository.GetByIdAsync(allocation.BatchId);
                    if (batch != null && batch.InventoryItemId == itemId)
                    {
                        decimal batchCost = batch.UnitCost * (decimal)allocation.Quantity;
                        result[batch.Id] = batchCost;
                        totalCost += batchCost;
                    }
                }
                
                result["total"] = totalCost;
            }
            else
            {
                // Calculate based on costing method
                switch (item.CostingMethod)
                {
                    case "FIFO":
                        result = await CalculateFIFOCostAsync(itemId, quantity);
                        break;
                    case "LIFO":
                        result = await CalculateLIFOCostAsync(itemId, quantity);
                        break;
                    case "WeightedAverage":
                        decimal avgCost = await CalculateAverageCostAsync(itemId);
                        result["total"] = avgCost * (decimal)quantity;
                        break;
                    case "LastPurchasePrice":
                        decimal latestCost = await CalculateLatestCostAsync(itemId);
                        result["total"] = latestCost * (decimal)quantity;
                        break;
                    default:
                        // Default to average cost
                        decimal defaultCost = await CalculateAverageCostAsync(itemId);
                        result["total"] = defaultCost * (decimal)quantity;
                        break;
                }
            }
            
            return result;
        }

        private async Task<Dictionary<string, decimal>> CalculateFIFOCostAsync(string itemId, double quantity)
        {
            var result = new Dictionary<string, decimal>
            {
                { "total", 0 }
            };
            
            var batches = await _batchRepository.GetByItemIdAsync(itemId);
            batches = batches.Where(b => b.RemainingQuantity > 0)
                            .OrderBy(b => b.ReceivedDate)
                            .ToList();
                            
            double remainingToConsume = quantity;
            decimal totalCost = 0;
            
            foreach (var batch in batches)
            {
                if (remainingToConsume <= 0)
                    break;
                    
                double quantityFromBatch = Math.Min(batch.RemainingQuantity, remainingToConsume);
                decimal batchCost = batch.UnitCost * (decimal)quantityFromBatch;
                
                result[batch.Id] = batchCost;
                totalCost += batchCost;
                remainingToConsume -= quantityFromBatch;
            }
            
            result["total"] = totalCost;
            return result;
        }

        private async Task<Dictionary<string, decimal>> CalculateLIFOCostAsync(string itemId, double quantity)
        {
            var result = new Dictionary<string, decimal>
            {
                { "total", 0 }
            };
            
            var batches = await _batchRepository.GetByItemIdAsync(itemId);
            batches = batches.Where(b => b.RemainingQuantity > 0)
                            .OrderByDescending(b => b.ReceivedDate)
                            .ToList();
                            
            double remainingToConsume = quantity;
            decimal totalCost = 0;
            
            foreach (var batch in batches)
            {
                if (remainingToConsume <= 0)
                    break;
                    
                double quantityFromBatch = Math.Min(batch.RemainingQuantity, remainingToConsume);
                decimal batchCost = batch.UnitCost * (decimal)quantityFromBatch;
                
                result[batch.Id] = batchCost;
                totalCost += batchCost;
                remainingToConsume -= quantityFromBatch;
            }
            
            result["total"] = totalCost;
            return result;
        }

        public async Task UpdateInventoryItemCostsAsync(string itemId, string userId)
        {
            var item = await _inventoryItemRepository.GetByIdAsync(itemId);
            if (item == null)
                return;
                
            // Calculate average cost
            decimal avgCost = await CalculateAverageCostAsync(itemId);
            
            // Calculate latest cost
            decimal latestCost = await CalculateLatestCostAsync(itemId);
            
            // Update the item
            item.UpdateAverageCost(avgCost, userId);
            item.UpdateCost(latestCost, userId);
            
            await _inventoryItemRepository.UpdateAsync(item);
        }

        public async Task<Dictionary<string, decimal>> CalculateInventoryValuationReportAsync(
            string locationId = null,
            List<string> categoryIds = null,
            DateTime? asOfDate = null)
        {
            var result = new Dictionary<string, decimal>
            {
                { "total", 0 }
            };
            
            // Process each category
            if (categoryIds != null && categoryIds.Any())
            {
                foreach (var categoryId in categoryIds)
                {
                    var categoryValue = await CalculateInventoryValueAsync(locationId, categoryId, asOfDate);
                    decimal total = categoryValue.Values.Sum();
                    
                    result[categoryId] = total;
                    result["total"] += total;
                }
            }
            else
            {
                // No categories specified, calculate total only
                var values = await CalculateInventoryValueAsync(locationId, null, asOfDate);
                result["total"] = values.Values.Sum();
            }
            
            return result;
        }
    }
}
