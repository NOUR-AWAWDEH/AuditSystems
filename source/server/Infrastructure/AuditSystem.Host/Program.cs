using AuditSystem.Application;
using AuditSystem.Auth;
using AuditSystem.BusinessLogic;
using AuditSystem.DataAccess;
using AuditSystem.Host.Extensions;
using AuditSystem.Host.Seed.Exceptions;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.OpenApi.Models;

// Load environment variables from .env
DotNetEnv.Env.Load();

var builder = WebApplication.CreateBuilder(new WebApplicationOptions
{
    Args = args,
    ContentRootPath = Directory.GetCurrentDirectory()
});

var services = builder.Services;
var environment = builder.Environment;
var configuration = builder.Configuration;

// --- Build Version Info ---
var buildVersion = "v1.2.3";
var buildDate = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss");

// --- Health checks ---
var healthChecksBuilder = services.AddHealthChecks()
    .AddCheck("Application", () => HealthCheckResult.Healthy("Application is running"));

// --- Host-level services ---
services.AddHostServices(buildVersion, buildDate);
services.AddAuthDependencies();
services.AddAdminSeeding(); // Seed admin user

// --- Application Core Services ---
services.AddApplication();
services.AddBusinessLogic();
services.AddAuthentication();
services.AddDataAccess(configuration, healthChecksBuilder, isProduction: environment.IsProduction());

services.ConfigureApiCookie();

// --- Data Protection ---
services.AddDataProtection()
        .PersistKeysToFileSystem(new DirectoryInfo(Path.Combine(environment.ContentRootPath, "keys")))
        .SetApplicationName("AuditSystem");

// --- File Upload Limit ---
services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 1_000_000_000; // 1 GB
});

// --- Kestrel Limits ---
builder.WebHost.ConfigureKestrel(options =>
{
    options.Limits.MaxRequestBodySize = 1_000_000_000; // 1 GB
});



var app = builder.Build();

// --- Middleware Pipeline ---

// Swagger JSON + UI
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Audit System API V1");
    c.RoutePrefix = "swagger";
    c.DocumentTitle = "Audit System API";
    c.HeadContent = $"<style>.swagger-ui .topbar:after{{content:'Last Build: {buildVersion} ({buildDate})';display:block;font-size:12px;color:#555;margin-top:5px;}}</style>";
});

// CORS
app.UseCors("AllowSpecificOrigin");

// Custom middleware pipeline
app.UseCustomMiddlewarePipeline();

// HTTPS redirection in production
if (environment.IsProduction())
{
    app.UseHttpsRedirection();
}

app.UseAuthentication();
app.UseAuthorization();
app.UseOutputCache();

// Health Checks
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

// Initialize database
DatabaseInitializer.InitializeDatabase(app.Services, environment.IsProduction());

// Seed admin AFTER app is built
await app.SeedAdminAsync();

// Run app
await app.RunAsync();
