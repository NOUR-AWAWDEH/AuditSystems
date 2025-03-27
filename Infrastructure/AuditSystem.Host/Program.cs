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

services.AddControllers();

var healthCheckBuilder = builder.Services.AddHealthChecks()
    .AddCheck("Application", () => HealthCheckResult.Healthy("Application is running"));

services.AddEndpointsApiExplorer();

var isProduction = builder.Environment.IsProduction();

services.AddClients(configuration, isProduction);
services.AddLogging();
services.AddApplication();
services.AddDomain();
services.AddAuthDependencies(configuration);
services.AddDataAccess(configuration, healthCheckBuilder);

services.AddCors((settings) =>
{
    settings.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

services.AddHttpContextAccessor();
services.AddDistributedMemoryCache();

// Increase FormOption limits
services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 1_000_000_000; // 1gb
});

// Increase Kestrel request body size limit
builder.WebHost.ConfigureKestrel(options =>
{
    options.Limits.MaxRequestBodySize = 1_000_000_000; // 1gb
});

var app = builder.Build();

app.MigrateDatabase();

app.UseSwagger(options =>
{
    options.RouteTemplate = "/openapi/{documentName}.json";
});
app.MapScalarApiReference();

app.UseScopedLogging();
app.UseErrorHandling();
app.UseValidation();

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

app.UseAuthentication();

app.UseAuthorization();

app.UseHttpsRedirection();

app.UseCors();

app.UseOutputCache();

app.MapControllers();

await app.RunAsync();