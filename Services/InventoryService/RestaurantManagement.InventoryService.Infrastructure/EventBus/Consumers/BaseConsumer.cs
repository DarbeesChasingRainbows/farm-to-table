using MassTransit;
using Microsoft.Extensions.Logging;

namespace RestaurantManagement.InventoryService.Infrastructure.EventBus.Consumers
{
    /// <summary>
    /// Base class for all event consumers.
    /// </summary>
    /// <typeparam name="TMessage">The type of the message.</typeparam>
    public abstract class BaseConsumer<TMessage> : IConsumer<TMessage>
        where TMessage : class
    {
        private readonly ILogger<BaseConsumer<TMessage>> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseConsumer{TMessage}"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        protected BaseConsumer(ILogger<BaseConsumer<TMessage>> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Consumes a message.
        /// </summary>
        /// <param name="context">The consume context.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task Consume(ConsumeContext<TMessage> context)
        {
            var messageType = typeof(TMessage).Name;
            var messageId = context.MessageId ?? Guid.NewGuid().ToString();

            try
            {
                _logger.LogInformation(
                    "Processing message {MessageType} with ID {MessageId}",
                    messageType,
                    messageId
                );

                await ConsumeMessage(context);

                _logger.LogInformation(
                    "Processed message {MessageType} with ID {MessageId} successfully",
                    messageType,
                    messageId
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error processing message {MessageType} with ID {MessageId}",
                    messageType,
                    messageId
                );
                throw;
            }
        }

        /// <summary>
        /// Consumes a message.
        /// </summary>
        /// <param name="context">The consume context.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        protected abstract Task ConsumeMessage(ConsumeContext<TMessage> context);
    }
}
