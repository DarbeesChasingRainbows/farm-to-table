using System.Threading;
using System.Threading.Tasks;

namespace RestaurantManagement.InventoryService.Domain.Events
{
    /// <summary>
    /// Interface for domain event handlers.
    /// </summary>
    /// <typeparam name="TEvent">The type of event to handle.</typeparam>
    public interface IEventHandler<in TEvent>
        where TEvent : IDomainEvent
    {
        /// <summary>
        /// Handles the specified event.
        /// </summary>
        /// <param name="event">The event to handle.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task HandleAsync(TEvent @event, CancellationToken cancellationToken = default);
    }
}
