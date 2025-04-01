using System.Reflection;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using RestaurantManagement.InventoryService.Application.Behaviors;
using RestaurantManagement.InventoryService.Application.Services;

namespace RestaurantManagement.InventoryService.Application
{
    /// <summary>
    /// Extension methods for registering application layer services with the DI container
    /// </summary>
    public static class DependencyInjection
    {
        /// <summary>
        /// Adds application layer services to the service collection
        /// </summary>
        /// <param name="services">The service collection</param>
        /// <returns>The modified service collection</returns>
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            // Add MediatR
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            });

            // Add AutoMapper
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            // Add FluentValidation
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            // Add MediatR behaviors
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(PerformanceBehavior<,>));

            // Add application services
            services.AddScoped<IInventoryService, InventoryService>();
            services.AddScoped<ITransactionService, TransactionService>();
            services.AddScoped<IReportingService, ReportingService>();

            return services;
        }
    }
}
