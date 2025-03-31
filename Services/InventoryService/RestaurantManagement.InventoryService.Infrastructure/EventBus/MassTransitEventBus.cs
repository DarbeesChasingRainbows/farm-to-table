using MassTransit;
using Microsoft.Extensions.Logging;

namespace RestaurantManagement.InventoryService.Infrastructure.EventBus
{
    /// <summary>
    /// MassTransit implementation of the event bus.
    /// </summary>
    public class MassTransitEventBus : IEventBus
    {
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly ISendEndpointProvider _sendEndpointProvider;
        private readonly ILogger<MassTransitEventBus> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="MassTransitEventBus"/> class.
        /// </summary>
        /// <param name="publishEndpoint">The publish endpoint.</param>
        /// <param name="sendEndpointProvider">The send endpoint provider.</param>
        /// <param name="logger">The logger.</param>
        public MassTransitEventBus(
            IPublishEndpoint publishEndpoint,
            ISendEndpointProvider sendEndpointProvider,
            ILogger<MassTransitEventBus> logger
        )
        {
            _publishEndpoint =
                publishEndpoint ?? throw new ArgumentNullException(nameof(publishEndpoint));
            _sendEndpointProvider =
                sendEndpointProvider
                ?? throw new ArgumentNullException(nameof(sendEndpointProvider));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Publishes an event to the message broker.
        /// </summary>
        /// <typeparam name="TEvent">The type of the event.</typeparam>
        /// <param name="event">The event to publish.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task PublishAsync<TEvent>(TEvent @event)
            where TEvent : class
        {
            try
            {
                _logger.LogInformation("Publishing event {EventType}", typeof(TEvent).Name);
                await _publishEndpoint.Publish(@event);
                _logger.LogInformation(
                    "Event {EventType} published successfully",
                    typeof(TEvent).Name
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error publishing event {EventType}", typeof(TEvent).Name);
                throw;
            }
        }

        /// <summary>
        /// Publishes an event to the message broker with a delay.
        /// </summary>
        /// <typeparam name="TEvent">The type of the event.</typeparam>
        /// <param name="event">The event to publish.</param>
        /// <param name="delay">The delay before publishing the event.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task PublishDelayedAsync<TEvent>(TEvent @event, TimeSpan delay)
            where TEvent : class
        {
            try
            {
                _logger.LogInformation(
                    "Publishing delayed event {EventType} with delay {Delay}",
                    typeof(TEvent).Name,
                    delay
                );
                await _publishEndpoint.Publish(@event, context => context.Delay = delay);
                _logger.LogInformation(
                    "Delayed event {EventType} scheduled successfully",
                    typeof(TEvent).Name
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error publishing delayed event {EventType}",
                    typeof(TEvent).Name
                );
                throw;
            }
        }

        /// <summary>
        /// Sends a command to a specific endpoint.
        /// </summary>
        /// <typeparam name="TCommand">The type of the command.</typeparam>
        /// <param name="command">The command to send.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task SendAsync<TCommand>(TCommand command)
            where TCommand : class
        {
            try
            {
                // Determine the endpoint based on the command type
                var endpoint = await _sendEndpointProvider.GetSendEndpoint(
                    new Uri($"queue:{typeof(TCommand).Name}")
                );

                _logger.LogInformation("Sending command {CommandType}", typeof(TCommand).Name);
                await endpoint.Send(command);
                _logger.LogInformation(
                    "Command {CommandType} sent successfully",
                    typeof(TCommand).Name
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending command {CommandType}", typeof(TCommand).Name);
                throw;
            }
        }
    }
}
