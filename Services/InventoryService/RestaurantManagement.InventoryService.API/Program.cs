using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantManagement.InventoryService.API.Filters;
using RestaurantManagement.InventoryService.API.GraphQL;
using RestaurantManagement.InventoryService.API.Middleware;
using RestaurantManagement.InventoryService.API.Services;
using RestaurantManagement.InventoryService.Application;
using RestaurantManagement.InventoryService.Infrastructure;
using RestaurantManagement.InventoryService.Infrastructure.Data.Context;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder
    .Services.AddControllers(options =>
    {
        // Add global filters
        options.Filters.Add<RequestLoggingFilter>();
        options.Filters.Add<PerformanceTrackingFilter>();
    })
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    });

// Configure APIs and Swagger documentation
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc(
        "v1",
        new() { Title = "Restaurant Management - Inventory Service API", Version = "v1" }
    );

    // Add XML comments for better documentation
    var xmlFilename = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFilename);
    if (File.Exists(xmlPath))
    {
        options.IncludeXmlComments(xmlPath);
    }
});

// Add Application Layer
builder.Services.AddApplication();

// Add Infrastructure Layer
builder.Services.AddInfrastructure(builder.Configuration);

// Configure health checks
builder
    .Services.AddHealthChecks()
    .AddDbContextCheck<InventoryDbContext>()
    .AddCheck<DatabaseHealthCheck>("database_health_check");

// Add API Versioning
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
});

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy(
        "AllowSpecificOrigins",
        policyBuilder =>
        {
            policyBuilder
                .WithOrigins(
                    builder.Configuration.GetSection("AllowedOrigins").Get<string[]>()
                        ?? Array.Empty<string>()
                )
                .AllowAnyHeader()
                .AllowAnyMethod();
        }
    );
});

// Add GraphQL
builder
    .Services.AddGraphQLServer()
    .AddQueryType<Query>()
    .AddMutationType<Mutation>()
    .AddFiltering()
    .AddSorting();

// Add gRPC Service
builder.Services.AddGrpc(options =>
{
    options.EnableDetailedErrors = builder.Environment.IsDevelopment();
    options.MaxReceiveMessageSize = 16 * 1024 * 1024; // 16 MB
    options.MaxSendMessageSize = 16 * 1024 * 1024; // 16 MB
});

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Inventory Service API v1");
        c.RoutePrefix = string.Empty; // Set Swagger UI at app root
    });
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/error");
    app.UseHsts();
}

// Apply migrations automatically in development
if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<InventoryDbContext>();

    app.Logger.LogInformation("Attempting to apply any pending migrations");
    try
    {
        dbContext.Database.Migrate();
        app.Logger.LogInformation("Migrations applied successfully");
    }
    catch (Exception ex)
    {
        app.Logger.LogError(ex, "An error occurred while applying migrations");
    }
}

// Global exception handling
app.UseMiddleware<ExceptionHandlingMiddleware>();

// Configure request pipeline
app.UseHttpsRedirection();
app.UseCors("AllowSpecificOrigins");
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

// Map endpoints
app.MapGraphQL(); // GraphQL endpoint at /graphql by default
app.MapGrpcService<InventoryGrpcService>(); // gRPC service

// Add health check endpoint
app.MapHealthChecks("/health");

app.MapControllers();

app.Run();

// Health check for database connection
public class DatabaseHealthCheck : Microsoft.Extensions.Diagnostics.HealthChecks.IHealthCheck
{
    private readonly InventoryDbContext _dbContext;

    public DatabaseHealthCheck(InventoryDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Microsoft.Extensions.Diagnostics.HealthChecks.HealthCheckResult> CheckHealthAsync(
        Microsoft.Extensions.Diagnostics.HealthChecks.HealthCheckContext context,
        CancellationToken cancellationToken = default
    )
    {
        try
        {
            // Simple check to verify database connection
            var canConnect = await _dbContext.Database.CanConnectAsync(cancellationToken);

            if (canConnect)
            {
                return Microsoft.Extensions.Diagnostics.HealthChecks.HealthCheckResult.Healthy(
                    "Database connection is healthy"
                );
            }

            return Microsoft.Extensions.Diagnostics.HealthChecks.HealthCheckResult.Unhealthy(
                "Cannot connect to the database"
            );
        }
        catch (Exception ex)
        {
            return Microsoft.Extensions.Diagnostics.HealthChecks.HealthCheckResult.Unhealthy(
                "Database health check failed",
                ex
            );
        }
    }
}
