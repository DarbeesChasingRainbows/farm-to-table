using System;

namespace RestaurantManagement.InventoryService.Domain.Exceptions
{
    /// <summary>
    /// Exception thrown when an invalid transaction type is specified.
    /// </summary>
    public class InvalidTransactionTypeException : DomainException
    {
        /// <summary>
        /// Gets the invalid transaction type.
        /// </summary>
        public string TransactionType { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidTransactionTypeException"/> class.
        /// </summary>
        /// <param name="transactionType">The invalid transaction type.</param>
        public InvalidTransactionTypeException(string transactionType)
            : base("TRX001", $"'{transactionType}' is not a valid transaction type.")
        {
            TransactionType = transactionType;
        }
    }

    /// <summary>
    /// Exception thrown when a destination location is required but not provided.
    /// </summary>
    public class MissingDestinationLocationException : DomainException
    {
        /// <summary>
        /// Gets the transaction ID.
        /// </summary>
        public string TransactionId { get; }

        /// <summary>
        /// Gets the transaction type.
        /// </summary>
        public string TransactionType { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MissingDestinationLocationException"/> class.
        /// </summary>
        /// <param name="transactionId">The transaction ID.</param>
        /// <param name="transactionType">The transaction type.</param>
        public MissingDestinationLocationException(string transactionId, string transactionType)
            : base(
                "TRX002",
                $"A destination location is required for transaction type '{transactionType}'."
            )
        {
            TransactionId = transactionId;
            TransactionType = transactionType;
        }
    }

    /// <summary>
    /// Exception thrown when attempting to add an item to a transaction with an invalid unit cost.
    /// </summary>
    public class InvalidUnitCostException : DomainException
    {
        /// <summary>
        /// Gets the transaction ID.
        /// </summary>
        public string TransactionId { get; }

        /// <summary>
        /// Gets the item ID.
        /// </summary>
        public string ItemId { get; }

        /// <summary>
        /// Gets the transaction type.
        /// </summary>
        public string TransactionType { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidUnitCostException"/> class.
        /// </summary>
        /// <param name="transactionId">The transaction ID.</param>
        /// <param name="itemId">The item ID.</param>
        /// <param name="transactionType">The transaction type.</param>
        /// <param name="message">The detailed error message.</param>
        public InvalidUnitCostException(
            string transactionId,
            string itemId,
            string transactionType,
            string message
        )
            : base("TRX003", message)
        {
            TransactionId = transactionId;
            ItemId = itemId;
            TransactionType = transactionType;
        }
    }

    /// <summary>
    /// Exception thrown when attempting to add an item with a negative or zero quantity.
    /// </summary>
    public class InvalidTransactionQuantityException : DomainException
    {
        /// <summary>
        /// Gets the transaction ID.
        /// </summary>
        public string TransactionId { get; }

        /// <summary>
        /// Gets the item ID.
        /// </summary>
        public string ItemId { get; }

        /// <summary>
        /// Gets the invalid quantity.
        /// </summary>
        public double Quantity { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidTransactionQuantityException"/> class.
        /// </summary>
        /// <param name="transactionId">The transaction ID.</param>
        /// <param name="itemId">The item ID.</param>
        /// <param name="quantity">The invalid quantity.</param>
        public InvalidTransactionQuantityException(
            string transactionId,
            string itemId,
            double quantity
        )
            : base(
                "TRX004",
                $"Transaction quantity must be greater than zero. Provided: {quantity}"
            )
        {
            TransactionId = transactionId;
            ItemId = itemId;
            Quantity = quantity;
        }
    }

    /// <summary>
    /// Exception thrown when a transaction is not found.
    /// </summary>
    public class TransactionNotFoundException : DomainException
    {
        /// <summary>
        /// Gets the transaction identifier that was not found.
        /// </summary>
        public string TransactionIdentifier { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TransactionNotFoundException"/> class.
        /// </summary>
        /// <param name="transactionId">The transaction ID that was not found.</param>
        public TransactionNotFoundException(string transactionId)
            : base("TRX005", $"Transaction with ID '{transactionId}' was not found.")
        {
            TransactionIdentifier = transactionId;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TransactionNotFoundException"/> class.
        /// </summary>
        /// <param name="referenceNumber">The reference number that was not found.</param>
        /// <param name="referenceType">The reference type.</param>
        public TransactionNotFoundException(string referenceNumber, string referenceType)
            : base(
                "TRX005",
                $"Transaction with reference number '{referenceNumber}' for reference type '{referenceType}' was not found."
            )
        {
            TransactionIdentifier = referenceNumber;
        }
    }

    /// <summary>
    /// Exception thrown when a transaction item is not found.
    /// </summary>
    public class TransactionItemNotFoundException : DomainException
    {
        /// <summary>
        /// Gets the transaction ID.
        /// </summary>
        public string TransactionId { get; }

        /// <summary>
        /// Gets the transaction item ID.
        /// </summary>
        public string TransactionItemId { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TransactionItemNotFoundException"/> class.
        /// </summary>
        /// <param name="transactionId">The transaction ID.</param>
        /// <param name="transactionItemId">The transaction item ID that was not found.</param>
        public TransactionItemNotFoundException(string transactionId, string transactionItemId)
            : base(
                "TRX006",
                $"Transaction item with ID '{transactionItemId}' was not found in transaction '{transactionId}'."
            )
        {
            TransactionId = transactionId;
            TransactionItemId = transactionItemId;
        }
    }
}
