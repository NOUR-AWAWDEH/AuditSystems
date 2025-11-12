using System.Text.Json.Serialization;
using AuditSystem.Contract.Interfaces.Cache;
using AuditSystem.DataAccess.Contexts;
using AuditSystem.Host.Caching;
using AuditSystem.Host.Seed.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AuditSystem.Host.Extensions;

public static class HostDependencies
{
    public static IServiceCollection AddHostServices(this IServiceCollection services, string buildVersion, string buildDate)
    {
        // Controllers with JSON string enum support
        services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });

        // API Explorer
        services.AddEndpointsApiExplorer();

        // Add output caching
        services.AddOutputCache();
        // Register CacheService
        services.AddSingleton<ICacheService, CacheService>();

        // Swagger
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
            {
                Title = "Audit System API V1",
                Version = "v1",
                Description = $"API for Audit System. Last Build: {buildVersion} ({buildDate})",
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

        // CORS - Consolidated configuration with environment variable support
        services.AddCors(options =>
        {
            options.AddPolicy("AllowSpecificOrigin", policy =>
            {
                // Get origins from .env / configuration
                var envOrigins = (Environment.GetEnvironmentVariable("CORS_ALLOWED_ORIGIN") ?? "")
                                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                                .Select(o => o.Trim()).ToArray();

                policy.WithOrigins(envOrigins)
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
            });
        });

        // HTTP Context & caching
        services.AddHttpContextAccessor();
        services.AddDistributedMemoryCache();

        return services;
    }

    public static void ConfigureApiCookie(this IServiceCollection services)
    {
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
    }

    /// <summary>
    /// Applies pending database migrations
    /// </summary>
    public static async Task MigrateDatabaseAsync(this IHost host)
    {
        using var scope = host.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        
        try
        {
            Console.WriteLine("Applying database migrations...");
            await dbContext.Database.MigrateAsync();
            Console.WriteLine("Database migrations applied successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error applying database migrations: {ex.Message}");
            throw;
        }
    }

    /// <summary>
    /// Seeds the admin user using the new seeding orchestrator
    /// </summary>
    public static async Task SeedAdminAsync(this IHost host)
    {
        using var scope = host.Services.CreateScope();
        var orchestrator = scope.ServiceProvider.GetRequiredService<IAdminSeedingOrchestrator>();
        await orchestrator.SeedAsync();
    }
}