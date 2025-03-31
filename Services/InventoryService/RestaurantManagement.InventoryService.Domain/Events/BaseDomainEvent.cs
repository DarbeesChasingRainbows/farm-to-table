using System;

namespace RestaurantManagement.InventoryService.Domain.Events
{
    /// <summary>
    /// Base class for all domain events.
    /// </summary>
    public abstract class BaseDomainEvent : IDomainEvent
    {
        /// <summary>
        /// Gets the timestamp when the event occurred.
        /// </summary>
        public DateTime Timestamp { get; }

        /// <summary>
        /// Gets the ID of the user who triggered the event.
        /// </summary>
        public string UserId { get; }

        /// <summary>
        /// Initializes a new instance of the BaseDomainEvent class.
        /// </summary>
        /// <param name="userId">The ID of the user who triggered the event.</param>
        protected BaseDomainEvent(string userId)
        {
            UserId = userId ?? throw new ArgumentNullException(nameof(userId));
            Timestamp = DateTime.UtcNow;
        }
    }
}
