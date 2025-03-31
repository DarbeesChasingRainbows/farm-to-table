using System;

namespace RestaurantManagement.InventoryService.Domain.Exceptions
{
    /// <summary>
    /// Exception thrown when attempting to modify a count sheet in an invalid state.
    /// </summary>
    public class InvalidCountSheetStateException : DomainException
    {
        /// <summary>
        /// Gets the count sheet ID.
        /// </summary>
        public string CountSheetId { get; }

        /// <summary>
        /// Gets the current state of the count sheet.
        /// </summary>
        public string CurrentState { get; }

        /// <summary>
        /// Gets the required state to perform the operation.
        /// </summary>
        public string RequiredState { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidCountSheetStateException"/> class.
        /// </summary>
        /// <param name="countSheetId">The count sheet ID.</param>
        /// <param name="currentState">The current state of the count sheet.</param>
        /// <param name="requiredState">The required state to perform the operation.</param>
        /// <param name="operation">The operation being attempted.</param>
        public InvalidCountSheetStateException(
            string countSheetId,
            string currentState,
            string requiredState,
            string operation
        )
            : base(
                "CNT001",
                $"Cannot {operation} a count sheet in state '{currentState}'. Required state: '{requiredState}'."
            )
        {
            CountSheetId = countSheetId;
            CurrentState = currentState;
            RequiredState = requiredState;
        }
    }

    /// <summary>
    /// Exception thrown when attempting to add a duplicate item to a count sheet.
    /// </summary>
    public class DuplicateCountSheetItemException : DomainException
    {
        /// <summary>
        /// Gets the count sheet ID.
        /// </summary>
        public string CountSheetId { get; }

        /// <summary>
        /// Gets the item ID.
        /// </summary>
        public string ItemId { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DuplicateCountSheetItemException"/> class.
        /// </summary>
        /// <param name="countSheetId">The count sheet ID.</param>
        /// <param name="itemId">The item ID.</param>
        /// <param name="itemName">The item name.</param>
        public DuplicateCountSheetItemException(string countSheetId, string itemId, string itemName)
            : base("CNT002", $"Item '{itemName}' is already included in the count sheet.")
        {
            CountSheetId = countSheetId;
            ItemId = itemId;
        }
    }

    /// <summary>
    /// Exception thrown when attempting to record a count for an item that does not exist in the count sheet.
    /// </summary>
    public class CountSheetItemNotFoundException : DomainException
    {
        /// <summary>
        /// Gets the count sheet ID.
        /// </summary>
        public string CountSheetId { get; }

        /// <summary>
        /// Gets the item ID.
        /// </summary>
        public string ItemId { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CountSheetItemNotFoundException"/> class.
        /// </summary>
        /// <param name="countSheetId">The count sheet ID.</param>
        /// <param name="itemId">The item ID.</param>
        public CountSheetItemNotFoundException(string countSheetId, string itemId)
            : base("CNT003", $"Item with ID '{itemId}' was not found in the count sheet.")
        {
            CountSheetId = countSheetId;
            ItemId = itemId;
        }
    }

    /// <summary>
    /// Exception thrown when attempting to record a count for a batch that does not exist in the count sheet item.
    /// </summary>
    public class BatchCountNotFoundException : DomainException
    {
        /// <summary>
        /// Gets the count sheet ID.
        /// </summary>
        public string CountSheetId { get; }

        /// <summary>
        /// Gets the count sheet item ID.
        /// </summary>
        public string CountSheetItemId { get; }

        /// <summary>
        /// Gets the batch ID.
        /// </summary>
        public string BatchId { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BatchCountNotFoundException"/> class.
        /// </summary>
        /// <param name="countSheetId">The count sheet ID.</param>
        /// <param name="countSheetItemId">The count sheet item ID.</param>
        /// <param name="batchId">The batch ID.</param>
        public BatchCountNotFoundException(
            string countSheetId,
            string countSheetItemId,
            string batchId
        )
            : base("CNT004", $"Batch with ID '{batchId}' was not found in the count sheet item.")
        {
            CountSheetId = countSheetId;
            CountSheetItemId = countSheetItemId;
            BatchId = batchId;
        }
    }

    /// <summary>
    /// Exception thrown when a count sheet is not found.
    /// </summary>
    public class CountSheetNotFoundException : DomainException
    {
        /// <summary>
        /// Gets the count sheet identifier that was not found.
        /// </summary>
        public string CountSheetIdentifier { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CountSheetNotFoundException"/> class.
        /// </summary>
        /// <param name="countSheetId">The count sheet ID that was not found.</param>
        public CountSheetNotFoundException(string countSheetId)
            : base("CNT005", $"Count sheet with ID '{countSheetId}' was not found.")
        {
            CountSheetIdentifier = countSheetId;
        }
    }

    /// <summary>
    /// Exception thrown when attempting to complete a count sheet with uncounted items.
    /// </summary>
    public class UncountedItemsException : DomainException
    {
        /// <summary>
        /// Gets the count sheet ID.
        /// </summary>
        public string CountSheetId { get; }

        /// <summary>
        /// Gets the number of uncounted items.
        /// </summary>
        public int UncountedItemsCount { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="UncountedItemsException"/> class.
        /// </summary>
        /// <param name="countSheetId">The count sheet ID.</param>
        /// <param name="uncountedItemsCount">The number of uncounted items.</param>
        public UncountedItemsException(string countSheetId, int uncountedItemsCount)
            : base(
                "CNT006",
                $"Cannot complete the count sheet. There are {uncountedItemsCount} items that have not been counted."
            )
        {
            CountSheetId = countSheetId;
            UncountedItemsCount = uncountedItemsCount;
        }
    }

    /// <summary>
    /// Exception thrown when attempting to approve a variance without providing a reason code.
    /// </summary>
    public class MissingVarianceReasonException : DomainException
    {
        /// <summary>
        /// Gets the count sheet ID.
        /// </summary>
        public string CountSheetId { get; }

        /// <summary>
        /// Gets the count sheet item ID.
        /// </summary>
        public string CountSheetItemId { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MissingVarianceReasonException"/> class.
        /// </summary>
        /// <param name="countSheetId">The count sheet ID.</param>
        /// <param name="countSheetItemId">The count sheet item ID.</param>
        public MissingVarianceReasonException(string countSheetId, string countSheetItemId)
            : base("CNT007", $"A reason code must be provided when approving a variance.")
        {
            CountSheetId = countSheetId;
            CountSheetItemId = countSheetItemId;
        }
    }
}
