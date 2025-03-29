using System;
using System.Collections.Generic;

namespace RestaurantManagement.InventoryService.Contracts.DTOs
{
    public class InventoryTransactionDto
    {
        public string Id { get; set; }
        public string Type { get; set; }
        public List<TransactionItemDto> Items { get; set; }
        public DateTime Timestamp { get; set; }
        public string ReferenceNumber { get; set; }
        public string ReferenceType { get; set; }
        public string LocationId { get; set; }
        public string LocationName { get; set; }
        public string DestinationLocationId { get; set; }
        public string DestinationLocationName { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Notes { get; set; }
        public decimal TotalValue { get; set; }
    }

    public class TransactionItemDto
    {
        public string ItemId { get; set; }
        public string ItemName { get; set; }
        public string ItemSku { get; set; }
        public double Quantity { get; set; }
        public string UnitOfMeasure { get; set; }
        public decimal UnitCost { get; set; }
        public decimal TotalCost { get; set; }
        public string BatchId { get; set; }
        public string BatchNumber { get; set; }
    }
}
