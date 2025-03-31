using System;

namespace RestaurantManagement.InventoryService.Domain.Exceptions
{
    /// <summary>
    /// Exception thrown when attempting to create an inventory item with a duplicate SKU.
    /// </summary>
    public class DuplicateSkuException : DomainException
    {
        /// <summary>
        /// Gets the SKU that caused the duplicate.
        /// </summary>
        public string Sku { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DuplicateSkuException"/> class.
        /// </summary>
        /// <param name="sku">The duplicate SKU.</param>
        public DuplicateSkuException(string sku)
            : base("INV001", $"An inventory item with SKU '{sku}' already exists.")
        {
            Sku = sku;
        }
    }

    /// <summary>
    /// Exception thrown when attempting to set an item as an alternative to itself.
    /// </summary>
    public class SelfReferenceAlternativeException : DomainException
    {
        /// <summary>
        /// Gets the ID of the inventory item.
        /// </summary>
        public string ItemId { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SelfReferenceAlternativeException"/> class.
        /// </summary>
        /// <param name="itemId">The ID of the inventory item.</param>
        public SelfReferenceAlternativeException(string itemId)
            : base(
                "INV002",
                $"An inventory item cannot be set as an alternative to itself. Item ID: {itemId}"
            )
        {
            ItemId = itemId;
        }
    }

    /// <summary>
    /// Exception thrown when attempting to set invalid thresholds.
    /// </summary>
    public class InvalidThresholdException : DomainException
    {
        /// <summary>
        /// Gets the ID of the inventory item.
        /// </summary>
        public string ItemId { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidThresholdException"/> class.
        /// </summary>
        /// <param name="itemId">The ID of the inventory item.</param>
        /// <param name="message">The detailed error message.</param>
        public InvalidThresholdException(string itemId, string message)
            : base("INV003", message)
        {
            ItemId = itemId;
        }
    }

    /// <summary>
    /// Exception thrown when attempting to use a discontinued inventory item.
    /// </summary>
    public class DiscontinuedItemException : DomainException
    {
        /// <summary>
        /// Gets the ID of the inventory item.
        /// </summary>
        public string ItemId { get; }

        /// <summary>
        /// Gets the name of the inventory item.
        /// </summary>
        public string ItemName { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DiscontinuedItemException"/> class.
        /// </summary>
        /// <param name="itemId">The ID of the inventory item.</param>
        /// <param name="itemName">The name of the inventory item.</param>
        public DiscontinuedItemException(string itemId, string itemName)
            : base("INV004", $"The inventory item '{itemName}' is discontinued and cannot be used.")
        {
            ItemId = itemId;
            ItemName = itemName;
        }
    }

    /// <summary>
    /// Exception thrown when an inventory item is not found.
    /// </summary>
    public class InventoryItemNotFoundException : DomainException
    {
        /// <summary>
        /// Gets the ID of the inventory item that was not found.
        /// </summary>
        public string ItemId { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="InventoryItemNotFoundException"/> class.
        /// </summary>
        /// <param name="itemId">The ID of the inventory item that was not found.</param>
        public InventoryItemNotFoundException(string itemId)
            : base("INV005", $"Inventory item with ID '{itemId}' was not found.")
        {
            ItemId = itemId;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InventoryItemNotFoundException"/> class with a specific identifier.
        /// </summary>
        /// <param name="identifier">The identifier value that was used to search for the item.</param>
        /// <param name="identifierType">The type of identifier used (SKU, Name, etc.).</param>
        public InventoryItemNotFoundException(string identifier, string identifierType)
            : base("INV005", $"Inventory item with {identifierType} '{identifier}' was not found.")
        {
            ItemId = identifier;
        }
    }
}
