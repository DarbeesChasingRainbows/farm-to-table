using System;
using System.Collections.Generic;

namespace RestaurantManagement.InventoryService.Contracts.DTOs
{
    public class InventoryUsageDto
    {
        public string ItemId { get; set; }
        public string ItemName { get; set; }
        public string Category { get; set; }
        public string UnitOfMeasure { get; set; }
        public List<PeriodUsageDto> UsageByPeriod { get; set; }
        public double TotalUsage { get; set; }
        public double AverageUsage { get; set; }
        public decimal TotalCost { get; set; }
    }

    public class PeriodUsageDto
    {
        public string Period { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public double Usage { get; set; }
        public decimal Cost { get; set; }
    }

    public class InventoryUsageReportDto
    {
        public List<InventoryUsageDto> Items { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string GroupBy { get; set; }
        public List<string> Periods { get; set; }
    }
}
