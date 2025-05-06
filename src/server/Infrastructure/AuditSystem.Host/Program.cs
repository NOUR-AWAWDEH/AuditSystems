using AuditSystem.Application;
using AuditSystem.Auth;
using AuditSystem.BusinessLogic;
using AuditSystem.DataAccess;
using AuditSystem.Host.Extensions;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Swashbuckle.AspNetCore.SwaggerGen; // Added for Swagger
using Swashbuckle.AspNetCore.SwaggerUI; // Added for Swagger UI

DotNetEnv.Env.Load();

var configBuilder = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddEnvironmentVariables()
    .AddInMemoryCollection();

var configuration = configBuilder.Build();

var builder = WebApplication.CreateBuilder(new WebApplicationOptions
{
    Args = args,
    ContentRootPath = Directory.GetCurrentDirectory()
});

builder.Configuration.AddEnvironmentVariables();

var services = builder.Services;
var environment = builder.Environment;

services.AddControllers();
services.AddEndpointsApiExplorer(); // Required for OpenAPI generation

services.AddSwaggerGen(options => // Added for Swagger
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Audit System API V1",
        Version = "v1"
    });
});

var healthCheckBuilder = services.AddHealthChecks()
    .AddCheck("Application", () => HealthCheckResult.Healthy("Application is running"));

services.AddClients(configuration, environment.IsProduction());
services.AddLogging();
services.AddApplication();
services.AddDomain();
services.AddAuthDependencies(configuration);
services.AddDataAccess(configuration, healthCheckBuilder, environment.IsProduction());

services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("https://auditsystem.onrender.com", "http://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

services.AddHttpContextAccessor();
services.AddDistributedMemoryCache();

services.AddDataProtection()
    .PersistKeysToFileSystem(new DirectoryInfo(Path.Combine(environment.ContentRootPath, "keys")))
    .SetApplicationName("AuditSystem");

services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 1_000_000_000;
});

builder.WebHost.ConfigureKestrel(options =>
{
    options.Limits.MaxRequestBodySize = 1_000_000_000;
});

var app = builder.Build();

app.MigrateDatabase();

// Configure Swagger
app.UseSwagger(options =>
{
    options.SerializeAsV2 = false; // Use OpenAPI v3
    options.RouteTemplate = "swagger/{documentName}/swagger.json";
});

app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Audit System API V1");
    options.RoutePrefix = "swagger"; // Swagger UI at /swagger
    options.DocumentTitle = "Audit System API";
});

app.UseCustomMiddlewarePipeline();

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

app.UseCors();

if (!environment.IsProduction())
{
    app.UseHttpsRedirection();
}

app.UseAuthentication();
app.UseAuthorization();
app.UseOutputCache();

app.MapControllers();

// Redirect root to Swagger UI
app.MapGet("/", context =>
{
    context.Response.Redirect("/swagger", permanent: false);
    return Task.CompletedTask;
});

await app.RunAsync();