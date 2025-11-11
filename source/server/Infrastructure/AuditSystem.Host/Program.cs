using AuditSystem.Application;
using AuditSystem.Auth;
using AuditSystem.BusinessLogic;
using AuditSystem.DataAccess;
using AuditSystem.Host.Extensions;
using AuditSystem.Host.Seed.Exceptions; // This namespace contains AddAdminSeeding
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Diagnostics.HealthChecks;

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

// Health checks
var healthChecksBuilder = services.AddHealthChecks()
    .AddCheck("Application", () => HealthCheckResult.Healthy("Application is running"));

// Host-level services
services.AddHostServices();
services.AddAuthDependencies();
services.AddAdminSeeding(); // Now this will work

// --- Application Core Services ---
services.AddApplication();
services.AddBusinessLogic();
services.AddAuthentication();
services.AddDataAccess(
    configuration,
    healthChecksBuilder,
    isProduction: environment.IsProduction()
);

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

// --- Swagger ---
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Audit System API V1");
    options.RoutePrefix = "swagger";
    options.DocumentTitle = "Audit System API";
    options.EnableDeepLinking();
    options.DisplayRequestDuration();
});

// --- Middleware Pipeline ---
app.UseCors("AllowSpecificOrigin");
app.UseCustomMiddlewarePipeline();

// Only redirect to HTTPS in production
if (environment.IsProduction())
{
    app.UseHttpsRedirection();
}

app.UseAuthentication();
app.UseAuthorization();
app.UseOutputCache();

// --- Health Checks ---
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

// --- Controllers ---
app.MapControllers();

// --- Root Redirect to Swagger ---
app.MapGet("/", context =>
{
    context.Response.Redirect("/swagger", permanent: false);
    return Task.CompletedTask;
});

// Seed admin user AFTER app is built
await app.SeedAdminAsync();

await app.RunAsync();