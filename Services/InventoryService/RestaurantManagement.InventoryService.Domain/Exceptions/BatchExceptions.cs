using System;

namespace RestaurantManagement.InventoryService.Domain.Exceptions
{
    /// <summary>
    /// Exception thrown when attempting to create a batch with an invalid expiration date.
    /// </summary>
    public class InvalidExpirationDateException : DomainException
    {
        /// <summary>
        /// Gets the received date.
        /// </summary>
        public DateTime ReceivedDate { get; }

        /// <summary>
        /// Gets the expiration date.
        /// </summary>
        public DateTime ExpirationDate { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidExpirationDateException"/> class.
        /// </summary>
        /// <param name="receivedDate">The received date.</param>
        /// <param name="expirationDate">The expiration date.</param>
        public InvalidExpirationDateException(DateTime receivedDate, DateTime expirationDate)
            : base(
                "BAT001",
                $"Expiration date ({expirationDate:d}) must be after received date ({receivedDate:d})."
            )
        {
            ReceivedDate = receivedDate;
            ExpirationDate = expirationDate;
        }
    }

    /// <summary>
    /// Exception thrown when attempting to set a remaining quantity greater than the initial quantity.
    /// </summary>
    public class ExcessiveRemainingQuantityException : DomainException
    {
        /// <summary>
        /// Gets the batch ID.
        /// </summary>
        public string BatchId { get; }

        /// <summary>
        /// Gets the requested remaining quantity.
        /// </summary>
        public double RemainingQuantity { get; }

        /// <summary>
        /// Gets the initial quantity of the batch.
        /// </summary>
        public double InitialQuantity { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExcessiveRemainingQuantityException"/> class.
        /// </summary>
        /// <param name="batchId">The batch ID.</param>
        /// <param name="remainingQuantity">The requested remaining quantity.</param>
        /// <param name="initialQuantity">The initial quantity of the batch.</param>
        public ExcessiveRemainingQuantityException(
            string batchId,
            double remainingQuantity,
            double initialQuantity
        )
            : base(
                "BAT002",
                $"Remaining quantity ({remainingQuantity}) cannot exceed initial quantity ({initialQuantity})."
            )
        {
            BatchId = batchId;
            RemainingQuantity = remainingQuantity;
            InitialQuantity = initialQuantity;
        }
    }

    /// <summary>
    /// Exception thrown when attempting to consume more from a batch than is available.
    /// </summary>
    public class InsufficientBatchQuantityException : DomainException
    {
        /// <summary>
        /// Gets the batch ID.
        /// </summary>
        public string BatchId { get; }

        /// <summary>
        /// Gets the requested quantity to consume.
        /// </summary>
        public double RequestedQuantity { get; }

        /// <summary>
        /// Gets the remaining quantity in the batch.
        /// </summary>
        public double RemainingQuantity { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="InsufficientBatchQuantityException"/> class.
        /// </summary>
        /// <param name="batchId">The batch ID.</param>
        /// <param name="requestedQuantity">The requested quantity to consume.</param>
        /// <param name="remainingQuantity">The remaining quantity in the batch.</param>
        public InsufficientBatchQuantityException(
            string batchId,
            double requestedQuantity,
            double remainingQuantity
        )
            : base(
                "BAT003",
                $"Cannot consume {requestedQuantity} from batch. Only {remainingQuantity} remaining."
            )
        {
            BatchId = batchId;
            RequestedQuantity = requestedQuantity;
            RemainingQuantity = remainingQuantity;
        }
    }

    /// <summary>
    /// Exception thrown when attempting to use an expired batch.
    /// </summary>
    public class ExpiredBatchException : DomainException
    {
        /// <summary>
        /// Gets the batch ID.
        /// </summary>
        public string BatchId { get; }

        /// <summary>
        /// Gets the batch number.
        /// </summary>
        public string BatchNumber { get; }

        /// <summary>
        /// Gets the expiration date of the batch.
        /// </summary>
        public DateTime ExpirationDate { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExpiredBatchException"/> class.
        /// </summary>
        /// <param name="batchId">The batch ID.</param>
        /// <param name="batchNumber">The batch number.</param>
        /// <param name="expirationDate">The expiration date of the batch.</param>
        public ExpiredBatchException(string batchId, string batchNumber, DateTime expirationDate)
            : base(
                "BAT004",
                $"Batch {batchNumber} has expired on {expirationDate:d} and cannot be used."
            )
        {
            BatchId = batchId;
            BatchNumber = batchNumber;
            ExpirationDate = expirationDate;
        }
    }

    /// <summary>
    /// Exception thrown when a batch is not found.
    /// </summary>
    public class BatchNotFoundException : DomainException
    {
        /// <summary>
        /// Gets the batch identifier that was not found.
        /// </summary>
        public string BatchIdentifier { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BatchNotFoundException"/> class.
        /// </summary>
        /// <param name="batchId">The batch ID that was not found.</param>
        public BatchNotFoundException(string batchId)
            : base("BAT005", $"Batch with ID '{batchId}' was not found.")
        {
            BatchIdentifier = batchId;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BatchNotFoundException"/> class.
        /// </summary>
        /// <param name="batchNumber">The batch number that was not found.</param>
        /// <param name="itemId">The inventory item ID.</param>
        public BatchNotFoundException(string batchNumber, string itemId)
            : base(
                "BAT005",
                $"Batch with number '{batchNumber}' for item '{itemId}' was not found."
            )
        {
            BatchIdentifier = batchNumber;
        }
    }
}
