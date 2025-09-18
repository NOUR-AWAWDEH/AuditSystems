using AuditSystem.Application;
using AuditSystem.Auth;
using AuditSystem.BusinessLogic;
using AuditSystem.DataAccess;
using AuditSystem.Host.Extensions;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Diagnostics.HealthChecks;

// Load environment variables
DotNetEnv.Env.Load();

var builder = WebApplication.CreateBuilder(new WebApplicationOptions
{
    Args = args,
    ContentRootPath = Directory.GetCurrentDirectory()
});

var services = builder.Services;
var environment = builder.Environment;
var configuration = builder.Configuration;
var healthChecksBuilder = services.AddHealthChecks()
    .AddCheck("Application", () => HealthCheckResult.Healthy("Application is running"));

// Host-level services
services.AddHostServices();
services.AddAuthDependencies();

// Configure Swagger, controllers, JSON, CORS are done in HostDependencies
services.AddApplication();
services.AddBusinessLogic();
services.AddAuthentication();
services.AddDataAccess(
    configuration,
    healthChecksBuilder,
    isProduction: environment.IsProduction()
);


services.ConfigureApiCookie();

// Data protection & file upload settings
services.AddDataProtection()
        .PersistKeysToFileSystem(new DirectoryInfo(Path.Combine(environment.ContentRootPath, "keys")))
        .SetApplicationName("AuditSystem");

services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 1_000_000_000; // 1 GB
});

builder.WebHost.ConfigureKestrel(options =>
{
    options.Limits.MaxRequestBodySize = 1_000_000_000; // 1 GB
});

var app = builder.Build();

// Auto-migrate database
//app.MigrateDatabase();

// Swagger UI
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Audit System API V1");
    c.RoutePrefix = "swagger";
    c.DocumentTitle = "Audit System API";
    c.EnableDeepLinking();
    c.DisplayRequestDuration();
});

// Middleware pipeline
app.UseCustomMiddlewarePipeline();
app.UseCors();
if (!environment.IsProduction()) app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseOutputCache();

// Health checks
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

// Controllers
app.MapControllers();

// Root redirect to Swagger
app.MapGet("/", context =>
{
    context.Response.Redirect("/swagger", permanent: false);
    return Task.CompletedTask;
});

await app.RunAsync();
