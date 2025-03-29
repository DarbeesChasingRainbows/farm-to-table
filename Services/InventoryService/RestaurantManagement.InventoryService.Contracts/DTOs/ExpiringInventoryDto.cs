using System;
using System.Collections.Generic;

namespace RestaurantManagement.InventoryService.Contracts.DTOs
{
    public class ExpiringInventoryDto
    {
        public List<ExpiringItemDto> ExpiringItems { get; set; }
        public decimal TotalValue { get; set; }
        public int TotalItemCount { get; set; }
        public int TotalBatchCount { get; set; }
    }

    public class ExpiringItemDto
    {
        public string ItemId { get; set; }
        public string ItemName { get; set; }
        public string SKU { get; set; }
        public string Category { get; set; }
        public string UnitOfMeasure { get; set; }
        public List<ExpiringBatchDto> Batches { get; set; }
        public double TotalQuantity { get; set; }
        public decimal TotalValue { get; set; }
    }

    public class ExpiringBatchDto
    {
        public string BatchId { get; set; }
        public string BatchNumber { get; set; }
        public DateTime ExpirationDate { get; set; }
        public int DaysUntilExpiration { get; set; }
        public double RemainingQuantity { get; set; }
        public decimal UnitCost { get; set; }
        public decimal TotalValue { get; set; }
        public string LocationId { get; set; }
        public string LocationName { get; set; }
    }
}
