using System;
using System.Collections.Generic;

namespace RestaurantManagement.InventoryService.Contracts.DTOs
{
    public class InventoryValueDto
    {
        public decimal TotalValue { get; set; }
        public List<CategoryValueDto> ValueByCategory { get; set; }
        public List<LocationValueDto> ValueByLocation { get; set; }
        public DateTime AsOfDate { get; set; }
    }

    public class CategoryValueDto
    {
        public string Category { get; set; }
        public decimal Value { get; set; }
        public double Percentage { get; set; }
    }

    public class LocationValueDto
    {
        public string LocationId { get; set; }
        public string LocationName { get; set; }
        public decimal Value { get; set; }
        public double Percentage { get; set; }
    }
}
