using Microsoft.Extensions.Logging;
using RestaurantManagement.InventoryService.Domain.Events;

namespace RestaurantManagement.InventoryService.Infrastructure.EventBus
{
    /// <summary>
    /// Implementation of the event publisher.
    /// </summary>
    public class EventPublisher : IEventPublisher
    {
        private readonly IEventBus _eventBus;
        private readonly ILogger<EventPublisher> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="EventPublisher"/> class.
        /// </summary>
        /// <param name="eventBus">The event bus.</param>
        /// <param name="logger">The logger.</param>
        public EventPublisher(IEventBus eventBus, ILogger<EventPublisher> logger)
        {
            _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Publishes a domain event.
        /// </summary>
        /// <param name="domainEvent">The domain event to publish.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task PublishAsync(IDomainEvent domainEvent)
        {
            try
            {
                _logger.LogInformation(
                    "Publishing domain event {EventType}",
                    domainEvent.GetType().Name
                );
                await _eventBus.PublishAsync(domainEvent);
                _logger.LogInformation(
                    "Domain event {EventType} published successfully",
                    domainEvent.GetType().Name
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error publishing domain event {EventType}",
                    domainEvent.GetType().Name
                );
                throw;
            }
        }
    }
}
