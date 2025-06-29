using AuditSystem.Application;
using AuditSystem.Auth;
using AuditSystem.BusinessLogic;
using AuditSystem.DataAccess;
using AuditSystem.Host.Extensions;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Diagnostics.HealthChecks;

// Load environment variables from .env file
DotNetEnv.Env.Load();

// Create configuration builder with environment variables
var configBuilder = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddEnvironmentVariables()
    .AddInMemoryCollection();

var configuration = configBuilder.Build();

// Create web application builder
var builder = WebApplication.CreateBuilder(new WebApplicationOptions
{
    Args = args,
    ContentRootPath = Directory.GetCurrentDirectory()
});

// Add configuration sources
builder.Configuration.AddEnvironmentVariables();

var services = builder.Services;
var environment = builder.Environment;

// Add controllers and API explorer
services.AddControllers();
services.AddEndpointsApiExplorer();
services.AddAuthDependencies();

// Configure Swagger
services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Audit System API V1",
        Version = "v1",
        Description = "API for Audit System",
        Contact = new Microsoft.OpenApi.Models.OpenApiContact
        {
            Name = "Support Team",
            Email = "support@auditsystem.com"
        }
    });

    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// Add health checks
var healthCheckBuilder = services.AddHealthChecks()
    .AddCheck("Application", () => HealthCheckResult.Healthy("Application is running"));

// Register application services
services.AddClients(configuration, environment.IsProduction());
services.AddLogging();
services.AddApplication();
services.AddBusinessLogic();
services.AddAuthentication();
services.AddDataAccess(configuration, healthCheckBuilder, environment.IsProduction());

// This is the CRITICAL FIX for API redirect issues
services.ConfigureApplicationCookie(options =>
{
    options.Events.OnRedirectToLogin = context =>
    {
        if (context.Request.Path.StartsWithSegments("/api"))
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return Task.CompletedTask;
        }

        context.Response.Redirect(context.RedirectUri);
        return Task.CompletedTask;
    };

    options.Events.OnRedirectToAccessDenied = context =>
    {
        if (context.Request.Path.StartsWithSegments("/api"))
        {
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            return Task.CompletedTask;
        }
        context.Response.Redirect(context.RedirectUri);
        return Task.CompletedTask;
    };
});

// Configure CORS
services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("https://auditsystem.onrender.com", "http://localhost:5173", "https://localhost:7218")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();      
    });
});

// Add HTTP context accessor and caching
services.AddHttpContextAccessor();
services.AddDistributedMemoryCache();

// Configure data protection
services.AddDataProtection()
    .PersistKeysToFileSystem(new DirectoryInfo(Path.Combine(environment.ContentRootPath, "keys")))
    .SetApplicationName("AuditSystem");

// Configure form options for file uploads
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

// Configure Swagger
app.UseSwagger(options =>
{
    options.RouteTemplate = "swagger/{documentName}/swagger.json";
});

app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Audit System API V1");
    options.RoutePrefix = "swagger";
    options.DocumentTitle = "Audit System API";
    options.EnableDeepLinking();
    options.DisplayRequestDuration();
});

// Use custom middleware pipeline
app.UseCustomMiddlewarePipeline();

// Configure health checks endpoint
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

// Enable HTTPS redirection in non-production environments
if (!environment.IsProduction())
{
    app.UseHttpsRedirection();
}

// Enable authentication and authorization
app.UseAuthentication();
app.UseAuthorization();

// Enable output caching
app.UseOutputCache();

// Map controllers
app.MapControllers();

// Redirect root to Swagger UI
app.MapGet("/", context =>
{
    context.Response.Redirect("/swagger", permanent: false);
    return Task.CompletedTask;
});

// Run the application
await app.RunAsync();