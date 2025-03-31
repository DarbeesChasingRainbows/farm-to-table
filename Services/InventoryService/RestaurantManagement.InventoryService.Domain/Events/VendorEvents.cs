using System;

namespace RestaurantManagement.InventoryService.Domain.Events
{
    public abstract class VendorEvent : BaseDomainEvent
    {
        public string VendorId { get; }

        protected VendorEvent(string vendorId, string userId)
            : base(userId)
        {
            VendorId = vendorId ?? throw new ArgumentNullException(nameof(vendorId));
        }
    }

    public class VendorCreatedEvent : VendorEvent
    {
        public string Name { get; }
        public string ContactName { get; }
        public string Email { get; }
        public string Phone { get; }

        public VendorCreatedEvent(
            string vendorId,
            string name,
            string contactName,
            string email,
            string phone,
            string userId
        )
            : base(vendorId, userId)
        {
            Name = name;
            ContactName = contactName;
            Email = email;
            Phone = phone;
        }
    }

    public class VendorDetailsUpdatedEvent : VendorEvent
    {
        public string Name { get; }
        public string ContactName { get; }
        public string Email { get; }
        public string Phone { get; }

        public VendorDetailsUpdatedEvent(
            string vendorId,
            string name,
            string contactName,
            string email,
            string phone,
            string userId
        )
            : base(vendorId, userId)
        {
            Name = name;
            ContactName = contactName;
            Email = email;
            Phone = phone;
        }
    }

    public class VendorAddressUpdatedEvent : VendorEvent
    {
        public string Street { get; }
        public string City { get; }
        public string State { get; }
        public string PostalCode { get; }
        public string Country { get; }

        public VendorAddressUpdatedEvent(
            string vendorId,
            string street,
            string city,
            string state,
            string postalCode,
            string country,
            string userId
        )
            : base(vendorId, userId)
        {
            Street = street;
            City = city;
            State = state;
            PostalCode = postalCode;
            Country = country;
        }
    }

    public class VendorAccountInfoUpdatedEvent : VendorEvent
    {
        public string AccountNumber { get; }
        public string PaymentTerms { get; }

        public VendorAccountInfoUpdatedEvent(
            string vendorId,
            string accountNumber,
            string paymentTerms,
            string userId
        )
            : base(vendorId, userId)
        {
            AccountNumber = accountNumber;
            PaymentTerms = paymentTerms;
        }
    }

    public class VendorNotesUpdatedEvent : VendorEvent
    {
        public string Notes { get; }

        public VendorNotesUpdatedEvent(string vendorId, string notes, string userId)
            : base(vendorId, userId)
        {
            Notes = notes;
        }
    }

    public class VendorDeactivatedEvent : VendorEvent
    {
        public VendorDeactivatedEvent(string vendorId, string userId)
            : base(vendorId, userId) { }
    }

    public class VendorActivatedEvent : VendorEvent
    {
        public VendorActivatedEvent(string vendorId, string userId)
            : base(vendorId, userId) { }
    }

    public class VendorItemAddedEvent : VendorEvent
    {
        public string VendorItemId { get; }
        public string ItemId { get; }
        public string VendorSku { get; }
        public decimal UnitCost { get; }
        public string UnitOfMeasure { get; }
        public double? MinOrderQuantity { get; }
        public int LeadTimeDays { get; }
        public bool IsPreferred { get; }

        public VendorItemAddedEvent(
            string vendorId,
            string vendorItemId,
            string itemId,
            string vendorSku,
            decimal unitCost,
            string unitOfMeasure,
            double? minOrderQuantity,
            int leadTimeDays,
            bool isPreferred,
            string userId
        )
            : base(vendorId, userId)
        {
            VendorItemId = vendorItemId;
            ItemId = itemId;
            VendorSku = vendorSku;
            UnitCost = unitCost;
            UnitOfMeasure = unitOfMeasure;
            MinOrderQuantity = minOrderQuantity;
            LeadTimeDays = leadTimeDays;
            IsPreferred = isPreferred;
        }
    }

    public class VendorItemRemovedEvent : VendorEvent
    {
        public string ItemId { get; }

        public VendorItemRemovedEvent(string vendorId, string itemId, string userId)
            : base(vendorId, userId)
        {
            ItemId = itemId;
        }
    }

    public class VendorItemUpdatedEvent : VendorEvent
    {
        public string VendorItemId { get; }
        public string ItemId { get; }
        public string VendorSku { get; }
        public decimal UnitCost { get; }
        public string UnitOfMeasure { get; }
        public double? MinOrderQuantity { get; }
        public int LeadTimeDays { get; }
        public bool IsPreferred { get; }

        public VendorItemUpdatedEvent(
            string vendorId,
            string vendorItemId,
            string itemId,
            string vendorSku,
            decimal unitCost,
            string unitOfMeasure,
            double? minOrderQuantity,
            int leadTimeDays,
            bool isPreferred,
            string userId
        )
            : base(vendorId, userId)
        {
            VendorItemId = vendorItemId;
            ItemId = itemId;
            VendorSku = vendorSku;
            UnitCost = unitCost;
            UnitOfMeasure = unitOfMeasure;
            MinOrderQuantity = minOrderQuantity;
            LeadTimeDays = leadTimeDays;
            IsPreferred = isPreferred;
        }
    }
}
