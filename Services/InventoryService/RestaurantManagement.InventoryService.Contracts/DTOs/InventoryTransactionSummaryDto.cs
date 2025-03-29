using System;

namespace RestaurantManagement.InventoryService.Contracts.DTOs
{
    public class InventoryTransactionSummaryDto
    {
        public string Id { get; set; }
        public string Type { get; set; }
        public DateTime Timestamp { get; set; }
        public string ReferenceNumber { get; set; }
        public string LocationName { get; set; }
        public string UserName { get; set; }
        public int ItemCount { get; set; }
        public decimal TotalValue { get; set; }
    }
}
