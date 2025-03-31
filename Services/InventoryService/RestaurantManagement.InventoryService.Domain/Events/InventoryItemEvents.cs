using System;

namespace RestaurantManagement.InventoryService.Domain.Events
{
    public abstract class InventoryItemEvent : BaseDomainEvent
    {
        public string ItemId { get; }

        protected InventoryItemEvent(string itemId, string userId)
            : base(userId)
        {
            ItemId = itemId ?? throw new ArgumentNullException(nameof(itemId));
        }
    }

    public class InventoryItemCreatedEvent : InventoryItemEvent
    {
        public string Name { get; }
        public string Description { get; }
        public string SKU { get; }
        public string Category { get; }
        public string UnitOfMeasure { get; }
        public double ReorderThreshold { get; }

        public InventoryItemCreatedEvent(
            string itemId,
            string name,
            string description,
            string sku,
            string category,
            string unitOfMeasure,
            double reorderThreshold,
            string userId
        )
            : base(itemId, userId)
        {
            Name = name;
            Description = description;
            SKU = sku;
            Category = category;
            UnitOfMeasure = unitOfMeasure;
            ReorderThreshold = reorderThreshold;
        }
    }

    public class InventoryItemDetailsUpdatedEvent : InventoryItemEvent
    {
        public string Name { get; }
        public string Description { get; }
        public string Category { get; }
        public string Subcategory { get; }
        public string StorageRequirements { get; }

        public InventoryItemDetailsUpdatedEvent(
            string itemId,
            string name,
            string description,
            string category,
            string subcategory,
            string storageRequirements,
            string userId
        )
            : base(itemId, userId)
        {
            Name = name;
            Description = description;
            Category = category;
            Subcategory = subcategory;
            StorageRequirements = storageRequirements;
        }
    }

    public class InventoryItemUnitOfMeasureUpdatedEvent : InventoryItemEvent
    {
        public string UnitOfMeasure { get; }

        public InventoryItemUnitOfMeasureUpdatedEvent(
            string itemId,
            string unitOfMeasure,
            string userId
        )
            : base(itemId, userId)
        {
            UnitOfMeasure = unitOfMeasure;
        }
    }

    public class InventoryItemThresholdsUpdatedEvent : InventoryItemEvent
    {
        public double ReorderThreshold { get; }
        public double MinStockLevel { get; }
        public double MaxStockLevel { get; }
        public int LeadTimeDays { get; }

        public InventoryItemThresholdsUpdatedEvent(
            string itemId,
            double reorderThreshold,
            double minStockLevel,
            double maxStockLevel,
            int leadTimeDays,
            string userId
        )
            : base(itemId, userId)
        {
            ReorderThreshold = reorderThreshold;
            MinStockLevel = minStockLevel;
            MaxStockLevel = maxStockLevel;
            LeadTimeDays = leadTimeDays;
        }
    }

    public class InventoryItemExpirationTrackingUpdatedEvent : InventoryItemEvent
    {
        public bool TrackExpiration { get; }

        public InventoryItemExpirationTrackingUpdatedEvent(
            string itemId,
            bool trackExpiration,
            string userId
        )
            : base(itemId, userId)
        {
            TrackExpiration = trackExpiration;
        }
    }

    public class InventoryItemDefaultVendorSetEvent : InventoryItemEvent
    {
        public string VendorId { get; }

        public InventoryItemDefaultVendorSetEvent(string itemId, string vendorId, string userId)
            : base(itemId, userId)
        {
            VendorId = vendorId;
        }
    }

    public class InventoryItemCostingMethodSetEvent : InventoryItemEvent
    {
        public string CostingMethod { get; }

        public InventoryItemCostingMethodSetEvent(
            string itemId,
            string costingMethod,
            string userId
        )
            : base(itemId, userId)
        {
            CostingMethod = costingMethod;
        }
    }

    public class InventoryItemAlternativeAddedEvent : InventoryItemEvent
    {
        public string AlternativeItemId { get; }

        public InventoryItemAlternativeAddedEvent(
            string itemId,
            string alternativeItemId,
            string userId
        )
            : base(itemId, userId)
        {
            AlternativeItemId = alternativeItemId;
        }
    }

    public class InventoryItemAlternativeRemovedEvent : InventoryItemEvent
    {
        public string AlternativeItemId { get; }

        public InventoryItemAlternativeRemovedEvent(
            string itemId,
            string alternativeItemId,
            string userId
        )
            : base(itemId, userId)
        {
            AlternativeItemId = alternativeItemId;
        }
    }

    public class InventoryItemDiscontinuedEvent : InventoryItemEvent
    {
        public InventoryItemDiscontinuedEvent(string itemId, string userId)
            : base(itemId, userId) { }
    }

    public class InventoryItemReactivatedEvent : InventoryItemEvent
    {
        public InventoryItemReactivatedEvent(string itemId, string userId)
            : base(itemId, userId) { }
    }

    public class InventoryItemCostUpdatedEvent : InventoryItemEvent
    {
        public decimal OldCost { get; }
        public decimal NewCost { get; }

        public InventoryItemCostUpdatedEvent(
            string itemId,
            decimal oldCost,
            decimal newCost,
            string userId
        )
            : base(itemId, userId)
        {
            OldCost = oldCost;
            NewCost = newCost;
        }
    }

    public class InventoryItemAverageCostUpdatedEvent : InventoryItemEvent
    {
        public decimal OldAverageCost { get; }
        public decimal NewAverageCost { get; }

        public InventoryItemAverageCostUpdatedEvent(
            string itemId,
            decimal oldAverageCost,
            decimal newAverageCost,
            string userId
        )
            : base(itemId, userId)
        {
            OldAverageCost = oldAverageCost;
            NewAverageCost = newAverageCost;
        }
    }

    public class InventoryItemLowStockEvent : InventoryItemEvent
    {
        public string LocationId { get; }
        public double CurrentQuantity { get; }
        public double ReorderThreshold { get; }

        public InventoryItemLowStockEvent(
            string itemId,
            string locationId,
            double currentQuantity,
            double reorderThreshold,
            string userId
        )
            : base(itemId, userId)
        {
            LocationId = locationId;
            CurrentQuantity = currentQuantity;
            ReorderThreshold = reorderThreshold;
        }
    }

    public class InventoryItemOutOfStockEvent : InventoryItemEvent
    {
        public string LocationId { get; }

        public InventoryItemOutOfStockEvent(string itemId, string locationId, string userId)
            : base(itemId, userId)
        {
            LocationId = locationId;
        }
    }
}
