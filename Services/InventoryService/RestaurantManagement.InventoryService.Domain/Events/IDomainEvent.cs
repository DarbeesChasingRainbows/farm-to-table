using System;

namespace RestaurantManagement.InventoryService.Domain.Events
{
    /// <summary>
    /// Interface for all domain events.
    /// </summary>
    public interface IDomainEvent
    {
        /// <summary>
        /// Gets the timestamp when the event occurred.
        /// </summary>
        DateTime Timestamp { get; }

        /// <summary>
        /// Gets the ID of the user who triggered the event.
        /// </summary>
        string UserId { get; }
    }
}
