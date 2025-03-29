using System;
using System.Collections.Generic;

namespace RestaurantManagement.InventoryService.Contracts.DTOs
{
    public class VendorDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string ContactName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public AddressDto Address { get; set; }
        public string AccountNumber { get; set; }
        public List<VendorItemDto> SuppliedItems { get; set; }
        public string PaymentTerms { get; set; }
        public string Notes { get; set; }
        public bool IsActive { get; set; }
    }

    public class VendorItemDto
    {
        public string ItemId { get; set; }
        public string ItemName { get; set; }
        public string VendorSku { get; set; }
        public decimal UnitCost { get; set; }
        public string UnitOfMeasure { get; set; }
        public double? MinOrderQuantity { get; set; }
        public int LeadTimeDays { get; set; }
        public bool IsPreferred { get; set; }
    }

    public class AddressDto
    {
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
    }
}
