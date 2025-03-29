using System;
using System.Collections.Generic;

namespace RestaurantManagement.InventoryService.Contracts.Queries
{
    public class GetBatchesQuery
    {
        public string ItemId { get; set; }
        public string LocationId { get; set; }
        public bool ActiveOnly { get; set; } = true;
        public bool IncludeEmpty { get; set; } = false;
        public DateTime? ExpirationBefore { get; set; }
        public DateTime? ExpirationAfter { get; set; }
        public string VendorId { get; set; }
        public string PurchaseOrderId { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
        public string SortBy { get; set; } = "ExpirationDate";
        public bool SortDescending { get; set; } = false;
    }
}
