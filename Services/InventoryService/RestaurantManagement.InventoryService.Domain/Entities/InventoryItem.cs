using System;
using System.Collections.Generic;

namespace RestaurantManagement.InventoryService.Domain.Entities
{
    public class InventoryItem
    {
        private readonly List<InventoryItem> _alternativeItems = new List<InventoryItem>();
        private readonly List<StockLevel> _stockLevels = new List<StockLevel>();
        private readonly List<Batch> _batches = new List<Batch>();

        // Required by EF Core
        protected InventoryItem() { }

        public InventoryItem(
            string id,
            string name,
            string description,
            string sku,
            string category,
            string unitOfMeasure,
            double reorderThreshold,
            string createdBy
        )
        {
            Id = id;
            Name = name;
            Description = description;
            SKU = sku;
            Category = category;
            UnitOfMeasure = unitOfMeasure;
            ReorderThreshold = reorderThreshold;
            CreatedBy = createdBy;
            CreatedAt = DateTime.UtcNow;
            IsActive = true;
        }

        public string Id { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public string SKU { get; private set; }
        public string Category { get; private set; }
        public string Subcategory { get; private set; }
        public string UnitOfMeasure { get; private set; }
        public string StorageRequirements { get; private set; }
        public double ReorderThreshold { get; private set; }
        public double MinStockLevel { get; private set; }
        public double MaxStockLevel { get; private set; }
        public int LeadTimeDays { get; private set; }
        public bool TrackExpiration { get; private set; }
        public string DefaultVendorId { get; private set; }
        public string CostingMethod { get; private set; }
        public decimal LastCost { get; private set; }
        public decimal AverageCost { get; private set; }
        public bool IsActive { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public string CreatedBy { get; private set; }
        public DateTime? LastModifiedAt { get; private set; }
        public string LastModifiedBy { get; private set; }

        public IReadOnlyCollection<InventoryItem> AlternativeItems =>
            _alternativeItems.AsReadOnly();
        public IReadOnlyCollection<StockLevel> StockLevels => _stockLevels.AsReadOnly();
        public IReadOnlyCollection<Batch> Batches => _batches.AsReadOnly();

        public void UpdateDetails(
            string name,
            string description,
            string category,
            string subcategory,
            string storageRequirements,
            string modifiedBy
        )
        {
            Name = name;
            Description = description;
            Category = category;
            Subcategory = subcategory;
            StorageRequirements = storageRequirements;
            LastModifiedBy = modifiedBy;
            LastModifiedAt = DateTime.UtcNow;
        }

        public void UpdateUnitOfMeasure(string unitOfMeasure, string modifiedBy)
        {
            UnitOfMeasure = unitOfMeasure;
            LastModifiedBy = modifiedBy;
            LastModifiedAt = DateTime.UtcNow;
        }

        public void SetThresholds(
            double reorderThreshold,
            double minStockLevel,
            double maxStockLevel,
            int leadTimeDays,
            string modifiedBy
        )
        {
            ReorderThreshold = reorderThreshold;
            MinStockLevel = minStockLevel;
            MaxStockLevel = maxStockLevel;
            LeadTimeDays = leadTimeDays;
            LastModifiedBy = modifiedBy;
            LastModifiedAt = DateTime.UtcNow;
        }

        public void SetExpirationTracking(bool trackExpiration, string modifiedBy)
        {
            TrackExpiration = trackExpiration;
            LastModifiedBy = modifiedBy;
            LastModifiedAt = DateTime.UtcNow;
        }

        public void SetDefaultVendor(string vendorId, string modifiedBy)
        {
            DefaultVendorId = vendorId;
            LastModifiedBy = modifiedBy;
            LastModifiedAt = DateTime.UtcNow;
        }

        public void SetCostingMethod(string costingMethod, string modifiedBy)
        {
            CostingMethod = costingMethod;
            LastModifiedBy = modifiedBy;
            LastModifiedAt = DateTime.UtcNow;
        }

        public void AddAlternativeItem(InventoryItem item)
        {
            if (item.Id == Id)
                return;

            if (!_alternativeItems.Exists(i => i.Id == item.Id))
                _alternativeItems.Add(item);
        }

        public void RemoveAlternativeItem(string itemId)
        {
            var item = _alternativeItems.Find(i => i.Id == itemId);
            if (item != null)
                _alternativeItems.Remove(item);
        }

        public void Discontinue(string modifiedBy)
        {
            IsActive = false;
            LastModifiedBy = modifiedBy;
            LastModifiedAt = DateTime.UtcNow;
        }

        public void Reactivate(string modifiedBy)
        {
            IsActive = true;
            LastModifiedBy = modifiedBy;
            LastModifiedAt = DateTime.UtcNow;
        }

        public void UpdateCost(decimal newCost, string modifiedBy)
        {
            LastCost = newCost;
            LastModifiedBy = modifiedBy;
            LastModifiedAt = DateTime.UtcNow;
        }

        public void UpdateAverageCost(decimal newAverageCost, string modifiedBy)
        {
            AverageCost = newAverageCost;
            LastModifiedBy = modifiedBy;
            LastModifiedAt = DateTime.UtcNow;
        }

        public StockLevel AddStockLevel(string locationId)
        {
            var existingLevel = _stockLevels.Find(sl => sl.LocationId == locationId);
            if (existingLevel != null)
                return existingLevel;

            var stockLevel = new StockLevel(Guid.NewGuid().ToString(), Id, locationId);

            _stockLevels.Add(stockLevel);
            return stockLevel;
        }

        public void AddBatch(Batch batch)
        {
            if (batch.InventoryItemId != Id)
                return;

            _batches.Add(batch);
        }

        public bool IsLowStock(string locationId)
        {
            var stockLevel = _stockLevels.Find(sl => sl.LocationId == locationId);
            if (stockLevel == null)
                return false;

            return stockLevel.AvailableQuantity < ReorderThreshold;
        }

        public bool IsOutOfStock(string locationId)
        {
            var stockLevel = _stockLevels.Find(sl => sl.LocationId == locationId);
            if (stockLevel == null)
                return true;

            return stockLevel.AvailableQuantity <= 0;
        }

        public double GetTotalStockQuantity()
        {
            double total = 0;
            foreach (var stockLevel in _stockLevels)
            {
                total += stockLevel.CurrentQuantity;
            }
            return total;
        }
    }
}
