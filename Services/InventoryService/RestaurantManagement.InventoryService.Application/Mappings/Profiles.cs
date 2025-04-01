using AutoMapper;
using RestaurantManagement.InventoryService.Application.Queries.InventoryItems;
using RestaurantManagement.InventoryService.Domain.Entities;

namespace RestaurantManagement.InventoryService.Application.Mappings
{
    /// <summary>
    /// Mapping profile for inventory-related entities
    /// </summary>
    public class InventoryMappingProfile : Profile
    {
        public InventoryMappingProfile()
        {
            // Inventory Item mappings
            CreateMap<InventoryItem, InventoryItemDto>();
            CreateMap<InventoryItem, InventoryItemListDto>();

            // Stock Level mappings
            CreateMap<StockLevel, StockLevelDto>();

            // Location mappings
            CreateMap<Location, LocationDto>();

            // Batch mappings
            CreateMap<Batch, BatchDto>();

            // Vendor mappings
            CreateMap<Vendor, VendorDto>();

            // Count Sheet mappings
            CreateMap<CountSheet, CountSheetDto>();
            CreateMap<CountSheetItem, CountSheetItemDto>();

            // Transaction mappings
            CreateMap<InventoryTransaction, InventoryTransactionDto>();
            CreateMap<InventoryTransactionItem, InventoryTransactionItemDto>();
        }
    }

    #region DTOs

    /// <summary>
    /// DTO for stock level
    /// </summary>
    public class StockLevelDto
    {
        public string Id { get; set; } = string.Empty;
        public string InventoryItemId { get; set; } = string.Empty;
        public string LocationId { get; set; } = string.Empty;
        public double CurrentQuantity { get; set; }
        public double ReservedQuantity { get; set; }
        public double AvailableQuantity { get; set; }
        public string? LastUpdatedBy { get; set; }
        public DateTime LastUpdatedAt { get; set; }
    }

    /// <summary>
    /// DTO for location
    /// </summary>
    public class LocationDto
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? LastModifiedBy { get; set; }
        public DateTime? LastModifiedAt { get; set; }
    }

    /// <summary>
    /// DTO for batch
    /// </summary>
    public class BatchDto
    {
        public string Id { get; set; } = string.Empty;
        public string InventoryItemId { get; set; } = string.Empty;
        public string LocationId { get; set; } = string.Empty;
        public string BatchNumber { get; set; } = string.Empty;
        public DateTime ExpirationDate { get; set; }
        public double InitialQuantity { get; set; }
        public double RemainingQuantity { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? LastModifiedBy { get; set; }
        public DateTime? LastModifiedAt { get; set; }
    }

    /// <summary>
    /// DTO for vendor
    /// </summary>
    public class VendorDto
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string ContactName { get; set; } = string.Empty;
        public string ContactEmail { get; set; } = string.Empty;
        public string ContactPhone { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? LastModifiedBy { get; set; }
        public DateTime? LastModifiedAt { get; set; }
    }

    /// <summary>
    /// DTO for count sheet
    /// </summary>
    public class CountSheetDto
    {
        public string Id { get; set; } = string.Empty;
        public string LocationId { get; set; } = string.Empty;
        public DateTime CountDate { get; set; }
        public string Status { get; set; } = string.Empty;
        public string? CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? CompletedBy { get; set; }
        public DateTime? CompletedAt { get; set; }
        public List<CountSheetItemDto> Items { get; set; } = new List<CountSheetItemDto>();
    }

    /// <summary>
    /// DTO for count sheet item
    /// </summary>
    public class CountSheetItemDto
    {
        public string Id { get; set; } = string.Empty;
        public string CountSheetId { get; set; } = string.Empty;
        public string InventoryItemId { get; set; } = string.Empty;
        public string InventoryItemName { get; set; } = string.Empty;
        public string SKU { get; set; } = string.Empty;
        public string UnitOfMeasure { get; set; } = string.Empty;
        public double ExpectedQuantity { get; set; }
        public double? ActualQuantity { get; set; }
        public double? Variance { get; set; }
        public string? Notes { get; set; }
    }

    /// <summary>
    /// DTO for inventory transaction
    /// </summary>
    public class InventoryTransactionDto
    {
        public string Id { get; set; } = string.Empty;
        public string TransactionType { get; set; } = string.Empty;
        public string? SourceLocationId { get; set; }
        public string? DestinationLocationId { get; set; }
        public string? VendorId { get; set; }
        public string ReferenceNumber { get; set; } = string.Empty;
        public DateTime TransactionDate { get; set; }
        public string Status { get; set; } = string.Empty;
        public string? Notes { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? CompletedBy { get; set; }
        public DateTime? CompletedAt { get; set; }
        public List<InventoryTransactionItemDto> Items { get; set; } =
            new List<InventoryTransactionItemDto>();
    }

    /// <summary>
    /// DTO for inventory transaction item
    /// </summary>
    public class InventoryTransactionItemDto
    {
        public string Id { get; set; } = string.Empty;
        public string TransactionId { get; set; } = string.Empty;
        public string InventoryItemId { get; set; } = string.Empty;
        public string InventoryItemName { get; set; } = string.Empty;
        public string SKU { get; set; } = string.Empty;
        public string UnitOfMeasure { get; set; } = string.Empty;
        public double Quantity { get; set; }
        public double? UnitPrice { get; set; }
        public string? BatchId { get; set; }
        public string? Notes { get; set; }
    }

    #endregion
}
