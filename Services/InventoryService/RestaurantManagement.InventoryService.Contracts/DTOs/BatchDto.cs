using System;

namespace RestaurantManagement.InventoryService.Contracts.DTOs
{
    public class BatchDto
    {
        public string Id { get; set; }
        public string ItemId { get; set; }
        public string BatchNumber { get; set; }
        public DateTime ReceivedDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public double InitialQuantity { get; set; }
        public double RemainingQuantity { get; set; }
        public decimal UnitCost { get; set; }
        public string LocationId { get; set; }
        public string LocationName { get; set; }
        public string VendorId { get; set; }
        public string VendorName { get; set; }
        public string PurchaseOrderId { get; set; }
        public bool IsExpired { get; set; }
        public int DaysUntilExpiration { get; set; }
    }
}
