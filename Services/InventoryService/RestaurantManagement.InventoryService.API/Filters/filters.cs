using System.Diagnostics;
using Microsoft.AspNetCore.Mvc.Filters;

namespace RestaurantManagement.InventoryService.API.Filters;

public class RequestLoggingFilter : IActionFilter
{
    private readonly ILogger<RequestLoggingFilter> _logger;

    public RequestLoggingFilter(ILogger<RequestLoggingFilter> logger)
    {
        _logger = logger;
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        _logger.LogInformation(
            "Request {Method} {Path} started at {Time}",
            context.HttpContext.Request.Method,
            context.HttpContext.Request.Path,
            DateTime.UtcNow
        );
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        _logger.LogInformation(
            "Request {Method} {Path} completed with status {StatusCode} at {Time}",
            context.HttpContext.Request.Method,
            context.HttpContext.Request.Path,
            context.HttpContext.Response.StatusCode,
            DateTime.UtcNow
        );
    }
}

public class PerformanceTrackingFilter : IActionFilter
{
    private readonly ILogger<PerformanceTrackingFilter> _logger;
    private Stopwatch? _stopwatch;

    public PerformanceTrackingFilter(ILogger<PerformanceTrackingFilter> logger)
    {
        _logger = logger;
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        _stopwatch = Stopwatch.StartNew();
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        if (_stopwatch != null)
        {
            _stopwatch.Stop();
            var elapsedMs = _stopwatch.ElapsedMilliseconds;

            var controllerName = context.Controller.GetType().Name;
            var actionName = context.ActionDescriptor.DisplayName;

            if (elapsedMs > 500) // Log warning for slow requests
            {
                _logger.LogWarning(
                    "Long running request: {Controller}.{Action} took {ElapsedMilliseconds}ms",
                    controllerName,
                    actionName,
                    elapsedMs
                );
            }
            else
            {
                _logger.LogInformation(
                    "Request: {Controller}.{Action} took {ElapsedMilliseconds}ms",
                    controllerName,
                    actionName,
                    elapsedMs
                );
            }
        }
    }
}
