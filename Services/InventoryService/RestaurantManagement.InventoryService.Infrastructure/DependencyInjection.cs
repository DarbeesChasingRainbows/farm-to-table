using System.Reflection;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RestaurantManagement.InventoryService.Application.Interfaces;
using RestaurantManagement.InventoryService.Application.Models;
using RestaurantManagement.InventoryService.Infrastructure.Data.Context;
using RestaurantManagement.InventoryService.Infrastructure.Data.Repositories;
using RestaurantManagement.InventoryService.Infrastructure.Data.Repositories.Interfaces;
using RestaurantManagement.InventoryService.Infrastructure.Data.UnitOfWork;
using RestaurantManagement.InventoryService.Infrastructure.EventBus;
using RestaurantManagement.InventoryService.Infrastructure.EventBus.Consumers;
using RestaurantManagement.InventoryService.Infrastructure.Services;
using RestaurantManagement.InventoryService.Infrastructure.Services.Settings;

namespace RestaurantManagement.InventoryService.Infrastructure
{
    /// <summary>
    /// Extension methods for registering infrastructure services.
    /// </summary>
    public static class DependencyInjection
    {
        /// <summary>
        /// Adds all infrastructure services to the service collection.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <param name="configuration">The configuration.</param>
        /// <returns>The service collection.</returns>
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration
        )
        {
            // Register DbContext
            services.AddDbContext<InventoryDbContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly(typeof(InventoryDbContext).Assembly.FullName)
                )
            );

            // Register Repositories
            services.AddScoped<IInventoryItemRepository, InventoryItemRepository>();
            services.AddScoped<IStockLevelRepository, StockLevelRepository>();
            services.AddScoped<ILocationRepository, LocationRepository>();
            services.AddScoped<IInventoryTransactionRepository, InventoryTransactionRepository>();
            services.AddScoped<IBatchRepository, BatchRepository>();
            services.AddScoped<IVendorRepository, VendorRepository>();
            services.AddScoped<ICountSheetRepository, CountSheetRepository>();

            // Register Unit of Work
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Register Event Bus and Message Handling
            services.AddScoped<IEventBus, MassTransitEventBus>();
            services.AddScoped<IEventPublisher, EventPublisher>();
            services.AddMassTransitRabbitMq(configuration);

            // Register Infrastructure Services
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<ICurrentUserService, CurrentUserService>();
            services.AddScoped<IDateTimeService, DateTimeService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<IFileStorageService, FileStorageService>();
            services.AddScoped<IBarcodeService, BarcodeService>();

            // Configure Settings
            services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));
            services.Configure<FileStorageSettings>(
                configuration.GetSection("FileStorageSettings")
            );

            return services;
        }

        /// <summary>
        /// Adds MassTransit with RabbitMQ to the service collection.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <param name="configuration">The configuration.</param>
        /// <returns>The service collection.</returns>
        private static IServiceCollection AddMassTransitRabbitMq(
            this IServiceCollection services,
            IConfiguration configuration
        )
        {
            services.AddMassTransit(x =>
            {
                // Register all consumers in the assembly
                x.AddConsumers(Assembly.GetExecutingAssembly());

                // Configure specific consumers manually for clarity
                x.AddConsumer<LowStockEventConsumer>();
                x.AddConsumer<BatchExpiringSoonEventConsumer>();
                x.AddConsumer<InventoryTransactionCompletedConsumer>();

                // Configure RabbitMQ
                x.UsingRabbitMq(
                    (context, cfg) =>
                    {
                        var host = configuration["RabbitMQ:Host"] ?? "localhost";
                        var username = configuration["RabbitMQ:Username"] ?? "guest";
                        var password = configuration["RabbitMQ:Password"] ?? "guest";

                        cfg.Host(
                            host,
                            "/",
                            h =>
                            {
                                h.Username(username);
                                h.Password(password);
                            }
                        );

                        // Configure retry policies
                        cfg.UseMessageRetry(r =>
                        {
                            r.Interval(3, TimeSpan.FromSeconds(5));
                        });

                        // Configure error handling
                        cfg.UseDelayedRedelivery(r =>
                        {
                            r.Intervals(
                                TimeSpan.FromMinutes(5),
                                TimeSpan.FromMinutes(15),
                                TimeSpan.FromMinutes(30)
                            );
                        });

                        cfg.UseInMemoryOutbox();

                        // Configure message endpoints
                        cfg.ConfigureEndpoints(
                            context,
                            new KebabCaseEndpointNameFormatter("restaurant-inventory", false)
                        );
                    }
                );
            });

            // Add MassTransit hosted services
            services.AddMassTransitHostedService();

            return services;
        }
    }
}
