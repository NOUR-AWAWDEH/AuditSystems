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
var configuration = builder.Configuration;
var services = builder.Services;

// Basic services
services.AddControllers();
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();
services.AddHttpContextAccessor();
services.AddDistributedMemoryCache();

// Health checks
var healthCheckBuilder = builder.Services.AddHealthChecks()
    .AddCheck("Application", () => HealthCheckResult.Healthy("Application is running"));

// CORS configuration
services.AddCors(settings =>
{
    settings.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// File size limits configuration
services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 1_000_000_000; // 1gb
});

builder.WebHost.ConfigureKestrel(options =>
{
    options.Limits.MaxRequestBodySize = 1_000_000_000; // 1gb
});

// Application services
var isProduction = builder.Environment.IsProduction();
services.AddClients(configuration, isProduction);
services.AddLogging();
services.AddApplication();
services.AddDomain();
services.AddDataAccess(configuration, healthCheckBuilder);
services.AddAuthDependencies(configuration);

var app = builder.Build();

// Database migration
app.MigrateDatabase();

// Swagger configuration
app.UseSwagger(options =>
{
    options.RouteTemplate = "/openapi/{documentName}.json";
});
app.MapScalarApiReference();

// Middleware pipeline
app.UseErrorHandling();
app.UseHttpsRedirection();  // Add this if not present
app.UseRouting();          // Add this if not present
app.UseCors();
app.UseAuthentication();    // Make sure this is before Authorization
app.UseAuthorization();
app.UseOutputCache();

// Health checks endpoint
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
                duration = e.Value.Duration.TotalMilliseconds + " ms"
            }),
            timestamp = DateTime.UtcNow
        };
        await context.Response.WriteAsync(System.Text.Json.JsonSerializer.Serialize(response));
    }
});

app.MapControllers();

await app.RunAsync();