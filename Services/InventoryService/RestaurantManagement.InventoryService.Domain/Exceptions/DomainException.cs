using System;

namespace RestaurantManagement.InventoryService.Domain.Exceptions
{
    /// <summary>
    /// Base exception for all domain-specific exceptions.
    /// </summary>
    public abstract class DomainException : Exception
    {
        /// <summary>
        /// Gets the error code associated with this exception.
        /// </summary>
        public string ErrorCode { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DomainException"/> class.
        /// </summary>
        /// <param name="errorCode">The error code associated with this exception.</param>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception.</param>
        protected DomainException(string errorCode, string message, Exception innerException = null)
            : base(message, innerException)
        {
            ErrorCode = errorCode ?? throw new ArgumentNullException(nameof(errorCode));
        }
    }
}
