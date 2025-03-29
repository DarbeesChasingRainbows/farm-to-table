using System;
using System.Collections.Generic;

namespace RestaurantManagement.InventoryService.Domain.Entities
{
    public class Vendor
    {
        private readonly List<VendorItem> _suppliedItems = new List<VendorItem>();

        // Required by EF Core
        protected Vendor() { }

        public Vendor(string id, string name, string contactName, string email, string phone)
        {
            Id = id;
            Name = name;
            ContactName = contactName;
            Email = email;
            Phone = phone;
            IsActive = true;
            CreatedAt = DateTime.UtcNow;
        }

        public string Id { get; private set; }
        public string Name { get; private set; }
        public string ContactName { get; private set; }
        public string Email { get; private set; }
        public string Phone { get; private set; }
        public string Street { get; private set; }
        public string City { get; private set; }
        public string State { get; private set; }
        public string PostalCode { get; private set; }
        public string Country { get; private set; }
        public string AccountNumber { get; private set; }
        public string PaymentTerms { get; private set; }
        public string Notes { get; private set; }
        public bool IsActive { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public string CreatedBy { get; private set; }
        public DateTime? LastModifiedAt { get; private set; }
        public string LastModifiedBy { get; private set; }

        public IReadOnlyCollection<VendorItem> SuppliedItems => _suppliedItems.AsReadOnly();

        public void UpdateDetails(
            string name,
            string contactName,
            string email,
            string phone,
            string modifiedBy
        )
        {
            Name = name;
            ContactName = contactName;
            Email = email;
            Phone = phone;
            LastModifiedBy = modifiedBy;
            LastModifiedAt = DateTime.UtcNow;
        }

        public void UpdateAddress(
            string street,
            string city,
            string state,
            string postalCode,
            string country,
            string modifiedBy
        )
        {
            Street = street;
            City = city;
            State = state;
            PostalCode = postalCode;
            Country = country;
            LastModifiedBy = modifiedBy;
            LastModifiedAt = DateTime.UtcNow;
        }

        public void UpdateAccountInfo(string accountNumber, string paymentTerms, string modifiedBy)
        {
            AccountNumber = accountNumber;
            PaymentTerms = paymentTerms;
            LastModifiedBy = modifiedBy;
            LastModifiedAt = DateTime.UtcNow;
        }

        public void UpdateNotes(string notes, string modifiedBy)
        {
            Notes = notes;
            LastModifiedBy = modifiedBy;
            LastModifiedAt = DateTime.UtcNow;
        }

        public void Deactivate(string modifiedBy)
        {
            IsActive = false;
            LastModifiedBy = modifiedBy;
            LastModifiedAt = DateTime.UtcNow;
        }

        public void Activate(string modifiedBy)
        {
            IsActive = true;
            LastModifiedBy = modifiedBy;
            LastModifiedAt = DateTime.UtcNow;
        }

        public void AddSuppliedItem(
            string itemId,
            string vendorSku,
            decimal unitCost,
            string unitOfMeasure,
            double? minOrderQuantity,
            int leadTimeDays,
            bool isPreferred
        )
        {
            var existingItem = _suppliedItems.Find(i => i.ItemId == itemId);

            if (existingItem != null)
            {
                existingItem.Update(
                    vendorSku,
                    unitCost,
                    unitOfMeasure,
                    minOrderQuantity,
                    leadTimeDays,
                    isPreferred
                );
            }
            else
            {
                var vendorItem = new VendorItem(
                    Guid.NewGuid().ToString(),
                    Id,
                    itemId,
                    vendorSku,
                    unitCost,
                    unitOfMeasure,
                    minOrderQuantity,
                    leadTimeDays,
                    isPreferred
                );

                _suppliedItems.Add(vendorItem);
            }
        }

        public void RemoveSuppliedItem(string itemId)
        {
            var item = _suppliedItems.Find(i => i.ItemId == itemId);
            if (item != null)
                _suppliedItems.Remove(item);
        }

        public VendorItem GetSuppliedItem(string itemId)
        {
            return _suppliedItems.Find(i => i.ItemId == itemId);
        }
    }

    public class VendorItem
    {
        // Required by EF Core
        protected VendorItem() { }

        public VendorItem(
            string id,
            string vendorId,
            string itemId,
            string vendorSku,
            decimal unitCost,
            string unitOfMeasure,
            double? minOrderQuantity,
            int leadTimeDays,
            bool isPreferred
        )
        {
            Id = id;
            VendorId = vendorId;
            ItemId = itemId;
            VendorSku = vendorSku;
            UnitCost = unitCost;
            UnitOfMeasure = unitOfMeasure;
            MinOrderQuantity = minOrderQuantity;
            LeadTimeDays = leadTimeDays;
            IsPreferred = isPreferred;
        }

        public string Id { get; private set; }
        public string VendorId { get; private set; }
        public string ItemId { get; private set; }
        public string VendorSku { get; private set; }
        public decimal UnitCost { get; private set; }
        public string UnitOfMeasure { get; private set; }
        public double? MinOrderQuantity { get; private set; }
        public int LeadTimeDays { get; private set; }
        public bool IsPreferred { get; private set; }

        // Navigation properties
        public Vendor Vendor { get; private set; }
        public InventoryItem Item { get; private set; }

        public void Update(
            string vendorSku,
            decimal unitCost,
            string unitOfMeasure,
            double? minOrderQuantity,
            int leadTimeDays,
            bool isPreferred
        )
        {
            VendorSku = vendorSku;
            UnitCost = unitCost;
            UnitOfMeasure = unitOfMeasure;
            MinOrderQuantity = minOrderQuantity;
            LeadTimeDays = leadTimeDays;
            IsPreferred = isPreferred;
        }
    }
}
