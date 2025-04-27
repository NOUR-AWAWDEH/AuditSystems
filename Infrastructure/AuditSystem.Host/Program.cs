using AuditSystem.Application;
using AuditSystem.Auth;
using AuditSystem.BusinessLogic;
using AuditSystem.DataAccess;
using AuditSystem.Host.Extensions;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Set up Configuration, Services
var configuration = builder.Configuration;
var services = builder.Services;
var environment = builder.Environment;

// Add Controllers
services.AddControllers();

// Add Health Checks
var healthCheckBuilder = services.AddHealthChecks()
    .AddCheck("Application", () => HealthCheckResult.Healthy("Application is running"));

// Add Swagger for API documentation
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

// Register Custom Application Services
services.AddClients(configuration, environment.IsProduction());
services.AddLogging();
services.AddApplication();
services.AddDomain();
services.AddAuthDependencies(configuration);
services.AddDataAccess(configuration, healthCheckBuilder, environment.IsProduction());

// Register Cross-Origin Resource Sharing (CORS) Policy
services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Allow access to HTTP Context
services.AddHttpContextAccessor();

// Add Caching
services.AddDistributedMemoryCache();

// Configure Form Options (for large file uploads)
services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 1_000_000_000; // 1 GB
});

// Configure Kestrel server limits
builder.WebHost.ConfigureKestrel(options =>
{
    options.Limits.MaxRequestBodySize = 1_000_000_000; // 1 GB
});

// Build the application
var app = builder.Build();

// Auto-migrate database on startup
app.MigrateDatabase();

// Enable Swagger/OpenAPI UI
if (environment.IsDevelopment() || environment.IsStaging())
{
    app.UseSwagger(options =>
    {
        options.RouteTemplate = "/openapi/{documentName}.json";
    });
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "Audit System API V1");
    });
}

// Map Scalar Reference API (Custom library)
app.MapScalarApiReference();

// Setup custom middlewares
app.UseScopedLogging();
app.UseErrorHandling();
app.UseValidation();

// Map Health Check Endpoint
app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = async (context, report) =>
    {
        context.Response.ContentType = "application/json";

        var response = new
        {
            status = report.Status.ToString(),
            checks = report.Entries.Select(e => new
            {
                name = e.Key,
                status = e.Value.Status.ToString(),
                description = e.Value.Description,
                duration = $"{e.Value.Duration.TotalMilliseconds} ms"
            }),
            timestamp = DateTime.UtcNow
        };

        await context.Response.WriteAsync(System.Text.Json.JsonSerializer.Serialize(response));
    }
});

// Enable CORS
app.UseCors();

// Redirect HTTP to HTTPS
app.UseHttpsRedirection();

// Enable Authentication & Authorization
app.UseAuthentication();
app.UseAuthorization();

// Enable Output Caching (if configured)
app.UseOutputCache();

// Map Controller Endpoints
app.MapControllers();

// Run the application
await app.RunAsync();
