using System;
using System.Collections.Generic;

namespace RestaurantManagement.InventoryService.Contracts.Commands
{
    public class ApproveVariancesCommand
    {
        public string CountSheetId { get; set; }
        public List<VarianceApprovalItem> Items { get; set; }
        public string ApprovedBy { get; set; }
        public DateTime ApprovalDate { get; set; }
        public string Notes { get; set; }
    }

    public class VarianceApprovalItem
    {
        public string ItemId { get; set; }
        public bool ApproveVariance { get; set; }
        public string ReasonCode { get; set; }
    }

    public class ApproveVariancesResult
    {
        public string AdjustmentTransactionId { get; set; }
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }
    }
}
