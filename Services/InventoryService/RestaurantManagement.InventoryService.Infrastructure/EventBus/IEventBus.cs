namespace RestaurantManagement.InventoryService.Infrastructure.EventBus
{
    /// <summary>
    /// Interface for the event bus.
    /// </summary>
    public interface IEventBus
    {
        /// <summary>
        /// Publishes an event to the message broker.
        /// </summary>
        /// <typeparam name="TEvent">The type of the event.</typeparam>
        /// <param name="event">The event to publish.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task PublishAsync<TEvent>(TEvent @event)
            where TEvent : class;

        /// <summary>
        /// Publishes an event to the message broker with a delay.
        /// </summary>
        /// <typeparam name="TEvent">The type of the event.</typeparam>
        /// <param name="event">The event to publish.</param>
        /// <param name="delay">The delay before publishing the event.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task PublishDelayedAsync<TEvent>(TEvent @event, TimeSpan delay)
            where TEvent : class;

        /// <summary>
        /// Sends a command to a specific endpoint.
        /// </summary>
        /// <typeparam name="TCommand">The type of the command.</typeparam>
        /// <param name="command">The command to send.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task SendAsync<TCommand>(TCommand command)
            where TCommand : class;
    }
}
