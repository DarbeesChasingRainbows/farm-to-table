using System;

namespace RestaurantManagement.InventoryService.Domain.Exceptions
{
    /// <summary>
    /// Exception thrown when attempting to set a negative stock quantity.
    /// </summary>
    public class NegativeStockQuantityException : DomainException
    {
        /// <summary>
        /// Gets the ID of the stock level.
        /// </summary>
        public string StockLevelId { get; }

        /// <summary>
        /// Gets the ID of the inventory item.
        /// </summary>
        public string ItemId { get; }

        /// <summary>
        /// Gets the requested quantity that caused the exception.
        /// </summary>
        public double RequestedQuantity { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="NegativeStockQuantityException"/> class.
        /// </summary>
        /// <param name="stockLevelId">The ID of the stock level.</param>
        /// <param name="itemId">The ID of the inventory item.</param>
        /// <param name="requestedQuantity">The requested quantity that would have resulted in negative stock.</param>
        public NegativeStockQuantityException(
            string stockLevelId,
            string itemId,
            double requestedQuantity
        )
            : base(
                "STK001",
                $"Cannot set a negative stock quantity. Requested quantity: {requestedQuantity}"
            )
        {
            StockLevelId = stockLevelId;
            ItemId = itemId;
            RequestedQuantity = requestedQuantity;
        }
    }

    /// <summary>
    /// Exception thrown when attempting to reserve more quantity than is available.
    /// </summary>
    public class InsufficientStockException : DomainException
    {
        /// <summary>
        /// Gets the ID of the stock level.
        /// </summary>
        public string StockLevelId { get; }

        /// <summary>
        /// Gets the ID of the inventory item.
        /// </summary>
        public string ItemId { get; }

        /// <summary>
        /// Gets the ID of the location.
        /// </summary>
        public string LocationId { get; }

        /// <summary>
        /// Gets the requested quantity.
        /// </summary>
        public double RequestedQuantity { get; }

        /// <summary>
        /// Gets the available quantity.
        /// </summary>
        public double AvailableQuantity { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="InsufficientStockException"/> class.
        /// </summary>
        /// <param name="stockLevelId">The ID of the stock level.</param>
        /// <param name="itemId">The ID of the inventory item.</param>
        /// <param name="locationId">The ID of the location.</param>
        /// <param name="requestedQuantity">The requested quantity.</param>
        /// <param name="availableQuantity">The available quantity.</param>
        public InsufficientStockException(
            string stockLevelId,
            string itemId,
            string locationId,
            double requestedQuantity,
            double availableQuantity
        )
            : base(
                "STK002",
                $"Insufficient stock available. Requested: {requestedQuantity}, Available: {availableQuantity}"
            )
        {
            StockLevelId = stockLevelId;
            ItemId = itemId;
            LocationId = locationId;
            RequestedQuantity = requestedQuantity;
            AvailableQuantity = availableQuantity;
        }
    }

    /// <summary>
    /// Exception thrown when attempting to release more quantity than is reserved.
    /// </summary>
    public class ExcessiveReleaseException : DomainException
    {
        /// <summary>
        /// Gets the ID of the stock level.
        /// </summary>
        public string StockLevelId { get; }

        /// <summary>
        /// Gets the quantity requested to be released.
        /// </summary>
        public double ReleaseQuantity { get; }

        /// <summary>
        /// Gets the currently reserved quantity.
        /// </summary>
        public double ReservedQuantity { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExcessiveReleaseException"/> class.
        /// </summary>
        /// <param name="stockLevelId">The ID of the stock level.</param>
        /// <param name="releaseQuantity">The quantity requested to be released.</param>
        /// <param name="reservedQuantity">The currently reserved quantity.</param>
        public ExcessiveReleaseException(
            string stockLevelId,
            double releaseQuantity,
            double reservedQuantity
        )
            : base(
                "STK003",
                $"Cannot release more quantity than is reserved. Release quantity: {releaseQuantity}, Reserved quantity: {reservedQuantity}"
            )
        {
            StockLevelId = stockLevelId;
            ReleaseQuantity = releaseQuantity;
            ReservedQuantity = reservedQuantity;
        }
    }

    /// <summary>
    /// Exception thrown when a stock level is not found.
    /// </summary>
    public class StockLevelNotFoundException : DomainException
    {
        /// <summary>
        /// Gets the ID of the stock level that was not found.
        /// </summary>
        public string StockLevelId { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="StockLevelNotFoundException"/> class.
        /// </summary>
        /// <param name="stockLevelId">The ID of the stock level.</param>
        public StockLevelNotFoundException(string stockLevelId)
            : base("STK004", $"Stock level with ID '{stockLevelId}' was not found.")
        {
            StockLevelId = stockLevelId;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StockLevelNotFoundException"/> class.
        /// </summary>
        /// <param name="itemId">The ID of the inventory item.</param>
        /// <param name="locationId">The ID of the location.</param>
        public StockLevelNotFoundException(string itemId, string locationId)
            : base(
                "STK004",
                $"Stock level for item '{itemId}' at location '{locationId}' was not found."
            )
        {
            StockLevelId = $"{itemId}_{locationId}";
        }
    }
}
