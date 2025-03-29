using System;
using System.Collections.Generic;

namespace RestaurantManagement.InventoryService.Contracts.Commands
{
    public class RecordCountsCommand
    {
        public string CountSheetId { get; set; }
        public List<CountedItemCommand> Items { get; set; }
        public string CountedBy { get; set; }
        public DateTime CompletedDate { get; set; }
        public string Notes { get; set; }
    }

    public class CountedItemCommand
    {
        public string ItemId { get; set; }
        public double CountedQuantity { get; set; }
        public string BatchId { get; set; }
    }

    public class RecordCountsResult
    {
        public string CountSheetId { get; set; }
        public bool Success { get; set; }
        public List<CountVariance> Variances { get; set; }
        public string ErrorMessage { get; set; }
    }

    public class CountVariance
    {
        public string ItemId { get; set; }
        public string ItemName { get; set; }
        public double SystemQuantity { get; set; }
        public double CountedQuantity { get; set; }
        public double Variance { get; set; }
        public double VariancePercentage { get; set; }
        public decimal EstimatedValue { get; set; }
    }
}
