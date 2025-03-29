using System;
using System.Collections.Generic;

namespace RestaurantManagement.InventoryService.Contracts.DTOs
{
    public class CountSheetDto
    {
        public string Id { get; set; }
        public string LocationId { get; set; }
        public string LocationName { get; set; }
        public List<string> Categories { get; set; }
        public string Status { get; set; }
        public DateTime CountDate { get; set; }
        public string RequestedBy { get; set; }
        public string RequestedByName { get; set; }
        public string CountedBy { get; set; }
        public string CountedByName { get; set; }
        public DateTime? CompletedDate { get; set; }
        public string ApprovedBy { get; set; }
        public string ApprovedByName { get; set; }
        public DateTime? ApprovalDate { get; set; }
        public string Notes { get; set; }
        public List<CountSheetItemDto> Items { get; set; }
        public int TotalItems { get; set; }
        public int CountedItems { get; set; }
        public int VarianceItems { get; set; }
        public decimal TotalVarianceValue { get; set; }
    }

    public class CountSheetItemDto
    {
        public string ItemId { get; set; }
        public string ItemName { get; set; }
        public string SKU { get; set; }
        public string Category { get; set; }
        public string UnitOfMeasure { get; set; }
        public double SystemQuantity { get; set; }
        public double? CountedQuantity { get; set; }
        public double? Variance { get; set; }
        public double? VariancePercentage { get; set; }
        public decimal UnitCost { get; set; }
        public decimal? VarianceValue { get; set; }
        public List<BatchCountDto> Batches { get; set; }
        public bool HasVariance { get; set; }
        public bool VarianceApproved { get; set; }
        public string VarianceReasonCode { get; set; }
    }

    public class BatchCountDto
    {
        public string BatchId { get; set; }
        public string BatchNumber { get; set; }
        public DateTime ExpirationDate { get; set; }
        public double SystemQuantity { get; set; }
        public double? CountedQuantity { get; set; }
    }
}
