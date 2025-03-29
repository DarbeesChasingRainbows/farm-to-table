using System;
using System.Collections.Generic;

namespace RestaurantManagement.InventoryService.Contracts.Queries
{
    public class GenerateInventoryReportQuery
    {
        public string ReportType { get; set; }  // Valuation, Stock, Transaction, Expiration, etc.
        public List<string> LocationIds { get; set; }
        public List<string> CategoryIds { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string GroupBy { get; set; }  // Category, Location, Vendor, etc.
        public string Format { get; set; } = "PDF";  // PDF, Excel, CSV
        public bool IncludeCharts { get; set; } = true;
    }
}
