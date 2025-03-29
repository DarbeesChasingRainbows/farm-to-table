using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RestaurantManagement.InventoryService.Domain.Entities;
using RestaurantManagement.InventoryService.Domain.Repositories;

namespace RestaurantManagement.InventoryService.Domain.Services
{
    public class InventoryPlanningService
    {
        private readonly IInventoryItemRepository _inventoryItemRepository;
        private readonly IStockLevelRepository _stockLevelRepository;
        private readonly IInventoryTransactionRepository _transactionRepository;
        private readonly IVendorRepository _vendorRepository;

        public InventoryPlanningService(
            IInventoryItemRepository inventoryItemRepository,
            IStockLevelRepository stockLevelRepository,
            IInventoryTransactionRepository transactionRepository,
            IVendorRepository vendorRepository)
        {
            _inventoryItemRepository = inventoryItemRepository;
            _stockLevelRepository = stockLevelRepository;
            _transactionRepository = transactionRepository;
            _vendorRepository = vendorRepository;
        }

        public async Task<List<ReorderSuggestion>> GenerateReorderSuggestionsAsync(
            string locationId,
            List<string> categoryIds = null)
        {
            var suggestions = new List<ReorderSuggestion>();
            
            // Get all stock levels for the location
            var stockLevels = await _stockLevelRepository.GetByLocationAsync(locationId);
            
            foreach (var stockLevel in stockLevels)
            {
                var item = await _inventoryItemRepository.GetByIdAsync(stockLevel.InventoryItemId);
                
                // Skip inactive items
                if (item == null || !item.IsActive)
                    continue;
                    
                // Filter by category if specified
                if (categoryIds != null && categoryIds.Any() && !categoryIds.Contains(item.Category))
                    continue;
                    
                // Check if stock is below reorder threshold
                if (stockLevel.AvailableQuantity <= item.ReorderThreshold)
                {
                    // Calculate how much to order
                    double orderQuantity = CalculateOrderQuantity(item, stockLevel);
                    
                    if (orderQuantity > 0)
                    {
                        // Get preferred vendor
                        var vendor = await GetPreferredVendorAsync(item.Id);
                        
                        var suggestion = new ReorderSuggestion
                        {
                            ItemId = item.Id,
                            ItemName = item.Name,
                            CurrentStock = stockLevel.AvailableQuantity,
                            ReorderThreshold = item.ReorderThreshold,
                            SuggestedOrderQuantity = orderQuantity,
                            UnitOfMeasure = item.UnitOfMeasure,
                            LocationId = locationId,
                            VendorId = vendor?.Id,
                            VendorName = vendor?.Name,
                            EstimatedCost = vendor != null ? 
                                await CalculateEstimatedCostAsync(item.Id, orderQuantity, vendor.Id) : 
                                item.LastCost * (decimal)orderQuantity
                        };
                        
                        suggestions.Add(suggestion);
                    }
                }
            }
            
            return suggestions;
        }

        private double CalculateOrderQuantity(InventoryItem item, StockLevel stockLevel)
        {
            // Basic calculation: order up to max level
            double orderUpToMax = item.MaxStockLevel - stockLevel.AvailableQuantity;
            
            // Adjust based on current usage rate
            double avgDailyUsage = CalculateAverageDailyUsage(item.Id).Result;
            double coverageDays = Math.Max(item.LeadTimeDays, 7); // At least a week's coverage
            double usageBasedOrder = avgDailyUsage * coverageDays - stockLevel.AvailableQuantity;
            
            // Take the larger of the two approaches, but at least enough to reach reorder threshold
            double minimumOrder = item.ReorderThreshold - stockLevel.AvailableQuantity + 1;
            
            return Math.Max(Math.Max(orderUpToMax, usageBasedOrder), minimumOrder);
        }

        private async Task<double> CalculateAverageDailyUsage(string itemId)
        {
            // Get consumption over the past 30 days
            DateTime startDate = DateTime.UtcNow.AddDays(-30);
            var consumptions = await _transactionRepository.GetByItemAndTypeAsync(
                itemId, 
                "Consumed", 
                startDate, 
                DateTime.UtcNow);
                
            double totalConsumed = consumptions.SelectMany(t => t.Items)
                .Where(i => i.InventoryItemId == itemId)
                .Sum(i => i.Quantity);
                
            return totalConsumed / 30.0; // Average per day
        }

        private async Task<Vendor> GetPreferredVendorAsync(string itemId)
        {
            var item = await _inventoryItemRepository.GetByIdAsync(itemId);
            
            if (!string.IsNullOrEmpty(item.DefaultVendorId))
            {
                return await _vendorRepository.GetByIdAsync(item.DefaultVendorId);
            }
            
            // Try to find a preferred vendor for this item
            var vendors = await _vendorRepository.GetByItemIdAsync(itemId);
            var preferredVendor = vendors.FirstOrDefault(v => 
                v.SuppliedItems.Any(i => i.ItemId == itemId && i.IsPreferred));
                
            return preferredVendor ?? vendors.FirstOrDefault();
        }

        private async Task<decimal> CalculateEstimatedCostAsync(
            string itemId, 
            double quantity, 
            string vendorId)
        {
            var vendor = await _vendorRepository.GetByIdAsync(vendorId);
            if (vendor == null)
                return 0;
                
            var vendorItem = vendor.SuppliedItems.FirstOrDefault(i => i.ItemId == itemId);
            if (vendorItem == null)
                return 0;
                
            return vendorItem.UnitCost * (decimal)quantity;
        }

        public async Task<Dictionary<string, List<ReorderSuggestion>>> GroupReorderSuggestionsByVendorAsync(
            List<ReorderSuggestion> suggestions)
        {
            var result = new Dictionary<string, List<ReorderSuggestion>>();
            
            foreach (var suggestion in suggestions)
            {
                if (string.IsNullOrEmpty(suggestion.VendorId))
                {
                    // Create a special group for items without vendors
                    if (!result.ContainsKey("unassigned"))
                        result["unassigned"] = new List<ReorderSuggestion>();
                        
                    result["unassigned"].Add(suggestion);
                }
                else
                {
                    if (!result.ContainsKey(suggestion.VendorId))
                        result[suggestion.VendorId] = new List<ReorderSuggestion>();
                        
                    result[suggestion.VendorId].Add(suggestion);
                }
            }
            
            return result;
        }

        public async Task<Dictionary<string, double>> CalculateInventoryTurnoverAsync(
            List<string> itemIds = null,
            string locationId = null,
            DateTime? startDate = null,
            DateTime? endDate = null)
        {
            startDate = startDate ?? DateTime.UtcNow.AddDays(-90);
            endDate = endDate ?? DateTime.UtcNow;
            
            var result = new Dictionary<string, double>();
            
            // Get items to analyze
            IEnumerable<InventoryItem> items;
            if (itemIds != null && itemIds.Any())
                items = await _inventoryItemRepository.GetByIdsAsync(itemIds);
            else
                items = await _inventoryItemRepository.GetAllActiveAsync();
                
            foreach (var item in items)
            {
                // Get consumption transactions for the period
                var consumptions = await _transactionRepository.GetByItemAndTypeAsync(
                    item.Id, 
                    "Consumed", 
                    startDate.Value, 
                    endDate.Value);
                    
                double totalConsumed = consumptions.SelectMany(t => t.Items)
                    .Where(i => i.InventoryItemId == item.Id)
                    .Sum(i => i.Quantity);
                    
                // Get average inventory level
                double averageInventory = await CalculateAverageInventoryLevelAsync(
                    item.Id, 
                    locationId, 
                    startDate.Value, 
                    endDate.Value);
                    
                // Calculate turnover rate (consumption / average inventory)
                double turnover = averageInventory > 0 ? totalConsumed / averageInventory : 0;
                
                result[item.Id] = turnover;
            }
            
            return result;
        }

        private async Task<double> CalculateAverageInventoryLevelAsync(
            string itemId, 
            string locationId,
            DateTime startDate,
            DateTime endDate)
        {
            // For simplicity, take current inventory level
            // In a real implementation, this would average snapshots over time
            var stockLevel = string.IsNullOrEmpty(locationId)
                ? await _stockLevelRepository.GetByItemAsync(itemId)
                : await _stockLevelRepository.GetByItemAndLocationAsync(itemId, locationId)
                      .ContinueWith(t => t.Result != null ? new List<StockLevel> { t.Result } : new List<StockLevel>());
                      
            return stockLevel.Sum(sl => sl.CurrentQuantity);
        }

        public async Task<Dictionary<string, double>> CalculateReorderPointsAsync(
            List<string> itemIds = null,
            string locationId = null)
        {
            var result = new Dictionary<string, double>();
            
            // Get items to analyze
            IEnumerable<InventoryItem> items;
            if (itemIds != null && itemIds.Any())
                items = await _inventoryItemRepository.GetByIdsAsync(itemIds);
            else
                items = await _inventoryItemRepository.GetAllActiveAsync();
                
            foreach (var item in items)
            {
                // Calculate average daily usage
                double avgDailyUsage = await CalculateAverageDailyUsage(item.Id);
                
                // Lead time from item or default to 7 days
                int leadTime = item.LeadTimeDays > 0 ? item.LeadTimeDays : 7;
                
                // Add safety stock (50% of lead time usage as a simple approach)
                double safetyStock = avgDailyUsage * leadTime * 0.5;
                
                // Reorder point = Lead time demand + Safety stock
                double reorderPoint = (avgDailyUsage * leadTime) + safetyStock;
                
                result[item.Id] = reorderPoint;
            }
            
            return result;
        }

        public async Task<Dictionary<string, object>> GenerateInventoryOptimizationReportAsync(
            string locationId = null,
            List<string> categoryIds = null)
        {
            var result = new Dictionary<string, object>();
            
            // Get suggestions for reordering
            var reorderSuggestions = await GenerateReorderSuggestionsAsync(
                locationId, 
                categoryIds);
                
            result["reorderSuggestions"] = reorderSuggestions;
            
            // Calculate turnover rates
            var turnoverRates = await CalculateInventoryTurnoverAsync(
                null, 
                locationId, 
                DateTime.UtcNow.AddDays(-90), 
                DateTime.UtcNow);
                
            result["turnoverRates"] = turnoverRates;
            
            // Identify slow-moving inventory (turnover < 1 over 90 days)
            var slowMovingItems = turnoverRates
                .Where(kv => kv.Value < 1.0)
                .Select(kv => kv.Key)
                .ToList();
                
            result["slowMovingItems"] = slowMovingItems;
            
            // Identify items with potential overstocking
            var overstockedItems = new List<string>();
            foreach (var itemId in turnoverRates.Keys)
            {
                var item = await _inventoryItemRepository.GetByIdAsync(itemId);
                if (item != null)
                {
                    var stockLevel = await _stockLevelRepository.GetByItemAndLocationAsync(
                        itemId, 
                        locationId ?? item.StockLevels.FirstOrDefault()?.LocationId);
                        
                    if (stockLevel != null && stockLevel.CurrentQuantity > item.MaxStockLevel)
                    {
                        overstockedItems.Add(itemId);
                    }
                }
            }
            
            result["overstockedItems"] = overstockedItems;
            
            return result;
        }
    }

    public class ReorderSuggestion
    {
        public string ItemId { get; set; }
        public string ItemName { get; set; }
        public double CurrentStock { get; set; }
        public double ReorderThreshold { get; set; }
        public double SuggestedOrderQuantity { get; set; }
        public string UnitOfMeasure { get; set; }
        public string LocationId { get; set; }
        public string VendorId { get; set; }
        public string VendorName { get; set; }
        public decimal EstimatedCost { get; set; }
    }
}
