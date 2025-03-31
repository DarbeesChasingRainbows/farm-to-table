using System;

namespace RestaurantManagement.InventoryService.Domain.Exceptions
{
    /// <summary>
    /// Exception thrown when attempting to create a vendor with a duplicate name.
    /// </summary>
    public class DuplicateVendorNameException : DomainException
    {
        /// <summary>
        /// Gets the vendor name that caused the duplicate.
        /// </summary>
        public string VendorName { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DuplicateVendorNameException"/> class.
        /// </summary>
        /// <param name="vendorName">The vendor name that caused the duplicate.</param>
        public DuplicateVendorNameException(string vendorName)
            : base("VEN001", $"A vendor with the name '{vendorName}' already exists.")
        {
            VendorName = vendorName;
        }
    }

    /// <summary>
    /// Exception thrown when attempting to add a duplicate item to a vendor's supplied items.
    /// </summary>
    public class DuplicateVendorItemException : DomainException
    {
        /// <summary>
        /// Gets the vendor ID.
        /// </summary>
        public string VendorId { get; }

        /// <summary>
        /// Gets the vendor name.
        /// </summary>
        public string VendorName { get; }

        /// <summary>
        /// Gets the item ID.
        /// </summary>
        public string ItemId { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DuplicateVendorItemException"/> class.
        /// </summary>
        /// <param name="vendorId">The vendor ID.</param>
        /// <param name="vendorName">The vendor name.</param>
        /// <param name="itemId">The item ID.</param>
        public DuplicateVendorItemException(string vendorId, string vendorName, string itemId)
            : base("VEN002", $"Vendor '{vendorName}' already supplies item with ID '{itemId}'.")
        {
            VendorId = vendorId;
            VendorName = vendorName;
            ItemId = itemId;
        }
    }

    /// <summary>
    /// Exception thrown when attempting to use an inactive vendor.
    /// </summary>
    public class InactiveVendorException : DomainException
    {
        /// <summary>
        /// Gets the vendor ID.
        /// </summary>
        public string VendorId { get; }

        /// <summary>
        /// Gets the vendor name.
        /// </summary>
        public string VendorName { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="InactiveVendorException"/> class.
        /// </summary>
        /// <param name="vendorId">The vendor ID.</param>
        /// <param name="vendorName">The vendor name.</param>
        public InactiveVendorException(string vendorId, string vendorName)
            : base("VEN003", $"Vendor '{vendorName}' is inactive and cannot be used.")
        {
            VendorId = vendorId;
            VendorName = vendorName;
        }
    }

    /// <summary>
    /// Exception thrown when attempting to set invalid vendor item details.
    /// </summary>
    public class InvalidVendorItemDetailsException : DomainException
    {
        /// <summary>
        /// Gets the vendor ID.
        /// </summary>
        public string VendorId { get; }

        /// <summary>
        /// Gets the item ID.
        /// </summary>
        public string ItemId { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidVendorItemDetailsException"/> class.
        /// </summary>
        /// <param name="vendorId">The vendor ID.</param>
        /// <param name="itemId">The item ID.</param>
        /// <param name="message">The detailed error message.</param>
        public InvalidVendorItemDetailsException(string vendorId, string itemId, string message)
            : base("VEN004", message)
        {
            VendorId = vendorId;
            ItemId = itemId;
        }
    }

    /// <summary>
    /// Exception thrown when a vendor is not found.
    /// </summary>
    public class VendorNotFoundException : DomainException
    {
        /// <summary>
        /// Gets the vendor identifier that was not found.
        /// </summary>
        public string VendorIdentifier { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="VendorNotFoundException"/> class.
        /// </summary>
        /// <param name="vendorId">The vendor ID that was not found.</param>
        public VendorNotFoundException(string vendorId)
            : base("VEN005", $"Vendor with ID '{vendorId}' was not found.")
        {
            VendorIdentifier = vendorId;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VendorNotFoundException"/> class with a vendor name.
        /// </summary>
        /// <param name="vendorName">The vendor name that was not found.</param>
        /// <param name="isName">Flag indicating that the identifier is a name.</param>
        public VendorNotFoundException(string vendorName, bool isName)
            : base("VEN005", $"Vendor with name '{vendorName}' was not found.")
        {
            VendorIdentifier = vendorName;
        }
    }

    /// <summary>
    /// Exception thrown when a vendor item is not found.
    /// </summary>
    public class VendorItemNotFoundException : DomainException
    {
        /// <summary>
        /// Gets the vendor ID.
        /// </summary>
        public string VendorId { get; }

        /// <summary>
        /// Gets the item ID.
        /// </summary>
        public string ItemId { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="VendorItemNotFoundException"/> class.
        /// </summary>
        /// <param name="vendorId">The vendor ID.</param>
        /// <param name="itemId">The item ID.</param>
        public VendorItemNotFoundException(string vendorId, string itemId)
            : base(
                "VEN006",
                $"Vendor with ID '{vendorId}' does not supply item with ID '{itemId}'."
            )
        {
            VendorId = vendorId;
            ItemId = itemId;
        }
    }
}
