// File: Infrastructure/RestaurantManagement.ServiceDefaults/Middleware/GlobalExceptionHandlingMiddleware.cs
namespace RestaurantManagement.ServiceDefaults.Middleware;

using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RestaurantManagement.Common.Exceptions;

/// <summary>
/// Middleware that catches all unhandled exceptions and returns appropriate HTTP responses.
/// </summary>
public class GlobalExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionHandlingMiddleware> _logger;
    private readonly IWebHostEnvironment _env;

    public GlobalExceptionHandlingMiddleware(
        RequestDelegate next,
        ILogger<GlobalExceptionHandlingMiddleware> logger,
        IWebHostEnvironment env
    )
    {
        _next = next;
        _logger = logger;
        _env = env;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        _logger.LogError(exception, "An unhandled exception occurred");

        var statusCode = GetStatusCode(exception);
        var response = new ProblemDetails
        {
            Status = statusCode,
            Title = GetTitle(exception),
            Detail = _env.IsDevelopment() ? exception.ToString() : "An error occurred."
        };

        if (_env.IsDevelopment())
        {
            response.Extensions["stackTrace"] = exception.StackTrace;
            response.Extensions["source"] = exception.Source;
        }

        context.Response.ContentType = "application/problem+json";
        context.Response.StatusCode = statusCode;

        return context.Response.WriteAsJsonAsync(response);
    }

    private static int GetStatusCode(Exception exception)
    {
        // Map exception types to appropriate status codes
        return exception switch
        {
            ValidationException => StatusCodes.Status400BadRequest,
            NotFoundException => StatusCodes.Status404NotFound,
            UnauthorizedAccessException => StatusCodes.Status401Unauthorized,
            ForbiddenAccessException => StatusCodes.Status403Forbidden,
            BusinessRuleViolationException => StatusCodes.Status422UnprocessableEntity,
            _ => StatusCodes.Status500InternalServerError
        };
    }

    private static string GetTitle(Exception exception)
    {
        // Get appropriate error title based on exception type
        return exception switch
        {
            ValidationException => "Validation Error",
            NotFoundException => "Resource Not Found",
            UnauthorizedAccessException => "Unauthorized",
            ForbiddenAccessException => "Forbidden",
            BusinessRuleViolationException => "Business Rule Violation",
            _ => "Server Error"
        };
    }
}

/// <summary>
/// Extension methods for GlobalExceptionHandlingMiddleware.
/// </summary>
public static class GlobalExceptionHandlingMiddlewareExtensions
{
    public static IApplicationBuilder UseGlobalExceptionHandling(this IApplicationBuilder app)
    {
        return app.UseMiddleware<GlobalExceptionHandlingMiddleware>();
    }
}
