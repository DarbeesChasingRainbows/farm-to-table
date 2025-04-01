using System.Diagnostics;
using MediatR;
using Microsoft.Extensions.Logging;
using RestaurantManagement.InventoryService.Application.Interfaces;

namespace RestaurantManagement.InventoryService.Application.Behaviors
{
    /// <summary>
    /// Behavior for logging requests and responses
    /// </summary>
    /// <typeparam name="TRequest">The request type</typeparam>
    /// <typeparam name="TResponse">The response type</typeparam>
    public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : notnull
    {
        private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;
        private readonly ICurrentUserService _currentUserService;
        private readonly IDateTimeService _dateTimeService;

        public LoggingBehavior(
            ILogger<LoggingBehavior<TRequest, TResponse>> logger,
            ICurrentUserService currentUserService,
            IDateTimeService dateTimeService
        )
        {
            _logger = logger;
            _currentUserService = currentUserService;
            _dateTimeService = dateTimeService;
        }

        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken
        )
        {
            var requestName = typeof(TRequest).Name;
            var userId = _currentUserService.UserId ?? "anonymous";
            var timestamp = _dateTimeService.Now;

            _logger.LogInformation(
                "Handling {RequestName} for user {UserId} at {Timestamp}",
                requestName,
                userId,
                timestamp
            );

            var stopwatch = Stopwatch.StartNew();
            var response = await next();
            stopwatch.Stop();

            _logger.LogInformation(
                "Handled {RequestName} for user {UserId} in {ElapsedMilliseconds}ms",
                requestName,
                userId,
                stopwatch.ElapsedMilliseconds
            );

            return response;
        }
    }

    /// <summary>
    /// Behavior for validating requests
    /// </summary>
    /// <typeparam name="TRequest">The request type</typeparam>
    /// <typeparam name="TResponse">The response type</typeparam>
    public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : notnull
    {
        private readonly IEnumerable<FluentValidation.IValidator<TRequest>> _validators;

        public ValidationBehavior(IEnumerable<FluentValidation.IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken
        )
        {
            if (_validators.Any())
            {
                var context = new FluentValidation.ValidationContext<TRequest>(request);

                var validationResults = await Task.WhenAll(
                    _validators.Select(v => v.ValidateAsync(context, cancellationToken))
                );

                var failures = validationResults
                    .SelectMany(r => r.Errors)
                    .Where(f => f != null)
                    .ToList();

                if (failures.Count != 0)
                {
                    throw new Application.Exceptions.ValidationException(failures);
                }
            }

            return await next();
        }
    }

    /// <summary>
    /// Behavior for performance tracking
    /// </summary>
    /// <typeparam name="TRequest">The request type</typeparam>
    /// <typeparam name="TResponse">The response type</typeparam>
    public class PerformanceBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : notnull
    {
        private readonly Stopwatch _timer;
        private readonly ILogger<TRequest> _logger;
        private readonly ICurrentUserService _currentUserService;

        public PerformanceBehavior(ILogger<TRequest> logger, ICurrentUserService currentUserService)
        {
            _timer = new Stopwatch();
            _logger = logger;
            _currentUserService = currentUserService;
        }

        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken
        )
        {
            _timer.Start();

            var response = await next();

            _timer.Stop();

            var elapsedMilliseconds = _timer.ElapsedMilliseconds;

            if (elapsedMilliseconds > 500)
            {
                var requestName = typeof(TRequest).Name;
                var userId = _currentUserService.UserId ?? "anonymous";

                _logger.LogWarning(
                    "Long running request: {Name} ({ElapsedMilliseconds} milliseconds) by {UserId}",
                    requestName,
                    elapsedMilliseconds,
                    userId
                );
            }

            return response;
        }
    }
}
