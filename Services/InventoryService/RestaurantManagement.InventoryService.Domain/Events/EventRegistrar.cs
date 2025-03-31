using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RestaurantManagement.InventoryService.Domain.Events
{
    /// <summary>
    /// Provides functionality for registering and handling domain events.
    /// </summary>
    public class EventRegistrar
    {
        private readonly List<IDomainEvent> _events = new List<IDomainEvent>();
        private readonly IEventPublisher _eventPublisher;

        /// <summary>
        /// Initializes a new instance of the EventRegistrar class.
        /// </summary>
        /// <param name="eventPublisher">The event publisher to use for publishing events.</param>
        public EventRegistrar(IEventPublisher eventPublisher)
        {
            _eventPublisher =
                eventPublisher ?? throw new ArgumentNullException(nameof(eventPublisher));
        }

        /// <summary>
        /// Gets the registered events.
        /// </summary>
        public IReadOnlyCollection<IDomainEvent> Events => _events.AsReadOnly();

        /// <summary>
        /// Registers a domain event.
        /// </summary>
        /// <param name="domainEvent">The domain event to register.</param>
        public void RegisterEvent(IDomainEvent domainEvent)
        {
            if (domainEvent == null)
            {
                throw new ArgumentNullException(nameof(domainEvent));
            }

            _events.Add(domainEvent);
        }

        /// <summary>
        /// Clears all registered events.
        /// </summary>
        public void ClearEvents()
        {
            _events.Clear();
        }

        /// <summary>
        /// Publishes all registered events.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task PublishEvents()
        {
            foreach (var domainEvent in _events)
            {
                await _eventPublisher.PublishAsync(domainEvent);
            }

            ClearEvents();
        }
    }

    /// <summary>
    /// Interface for publishing domain events.
    /// </summary>
    public interface IEventPublisher
    {
        /// <summary>
        /// Publishes a domain event.
        /// </summary>
        /// <param name="domainEvent">The domain event to publish.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task PublishAsync(IDomainEvent domainEvent);
    }
}
