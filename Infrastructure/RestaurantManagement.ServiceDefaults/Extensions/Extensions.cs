// File: Infrastructure/RestaurantManagement.ServiceDefaults/Extensions.cs
namespace Microsoft.Extensions.Hosting;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.ServiceDiscovery;
using RestaurantManagement.ServiceDefaults.Middleware;
using RestaurantManagement.ServiceDefaults.Services;

/// <summary>
/// Extensions for adding service defaults to ASP.NET Core applications.
/// </summary>
public static class Extensions
{
    public static IHostApplicationBuilder AddServiceDefaults(this IHostApplicationBuilder builder)
    {
        // Configure observability
        builder.ConfigureRestaurantObservability();

        // Add health checks
        builder.AddRestaurantHealthChecks(builder.Configuration);

        // Add service discovery
        builder.Services.AddRestaurantServiceDiscovery(builder.Configuration);

        // Configure HTTP client defaults
        builder.Services.ConfigureHttpClientDefaults(http =>
        {
            // Add resilience
            http.AddRestaurantResilienceHandler();

            // Add service discovery
            http.AddServiceDiscovery();
        });

        // Add common application services
        builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
        builder.Services.AddScoped<IDateTimeService, DateTimeService>();

        // Add common middleware
        builder.Services.AddSingleton<GlobalExceptionHandlingMiddleware>();

        // Add authentication and authorization if configured
        if (builder.Configuration.GetSection("Authentication").Exists())
        {
            builder.Services.AddRestaurantAuthentication(builder.Configuration);
        }

        // Add API documentation if enabled
        if (builder.Configuration.GetValue<bool>("EnableSwagger", defaultValue: true))
        {
            builder.Services.AddRestaurantApiDocumentation(builder.Configuration);
        }

        return builder;
    }

    public static WebApplication MapDefaultEndpoints(this WebApplication app)
    {
        // Add health check endpoints
        app.UseRestaurantHealthChecks(app.Environment);

        // Add global exception handling
        app.UseGlobalExceptionHandling();

        // Add API documentation if enabled
        if (app.Configuration.GetValue<bool>("EnableSwagger", defaultValue: true))
        {
            var apiVersionDescriptionProvider =
                app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
            app.UseRestaurantApiDocumentation(apiVersionDescriptionProvider);
        }

        return app;
    }
}
