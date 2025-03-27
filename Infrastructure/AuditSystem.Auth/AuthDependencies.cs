using AuditSystem.Auth.Authentication;
using AuditSystem.Auth.Services.Account;
using AuditSystem.Auth.Services.Authentication;
using AuditSystem.Auth.Services.Email;
using AuditSystem.Auth.Services.Password.PasswordToken;
using AuditSystem.Auth.Services.Password.ResetPassword;
using AuditSystem.Auth.Services.Registration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Threading.RateLimiting;

namespace AuditSystem.Auth;

public static class AuthDependencies
{
    public static IServiceCollection AddAuthDependencies(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Register authentication-related services
        services.AddScoped<ICustomAuthenticationService, CustomAuthenticationService>();
        services.AddScoped<IRegistrationService, RegistrationService>();
        services.AddScoped<IPasswordResetService, PasswordResetService>();
        services.AddScoped<IPasswordTokenService, PasswordTokenService>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<IAccountService, AccountService>();



        // JWT Settings
        var jwtSettings = new JwtSettings
        {
            Key = configuration["JwtSettings:Key"] ?? throw new ArgumentNullException("JwtSettings:Key", "JwtSettings:Key is missing in configuration"),
            Issuer = configuration["JwtSettings:Issuer"] ?? throw new ArgumentNullException("JwtSettings:Issuer", "JwtSettings:Issuer is missing in configuration"),
            Audience = configuration["JwtSettings:Audience"] ?? throw new ArgumentNullException("JwtSettings:Audience", "JwtSettings:Audience is missing in configuration"),
            ExpirationMinutes = int.TryParse(configuration["JwtSettings:ExpirationMinutes"], out int expirationMinutes)
                ? expirationMinutes
                : throw new ArgumentException("Invalid or missing JwtSettings:ExpirationMinutes in configuration", "JwtSettings:ExpirationMinutes")
        };

        services.AddSingleton(jwtSettings);

        // JWT Authentication
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key)),
                ValidateIssuer = true,
                ValidIssuer = jwtSettings.Issuer,
                ValidateAudience = true,
                ValidAudience = jwtSettings.Audience,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero // Stricter token expiration
            };
        });

        // Rate Limiting
        services.AddRateLimiter(options =>
        {
            options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

            // Optional: Global rate limit (e.g., 1000 requests per minute across all IPs)
            options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
                RateLimitPartition.GetFixedWindowLimiter(
                    partitionKey: "global",
                    factory: _ => new FixedWindowRateLimiterOptions
                    {
                        PermitLimit = 1000,
                        Window = TimeSpan.FromMinutes(1),
                        QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                        QueueLimit = 0
                    }));

            // Per-IP rate limit for login endpoints
            options.AddPolicy("loginPolicy", httpContext =>
                RateLimitPartition.GetFixedWindowLimiter(
                    partitionKey: httpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown",
                    factory: _ => new FixedWindowRateLimiterOptions
                    {
                        PermitLimit = 8,              // 8 requests allowed
                        Window = TimeSpan.FromMinutes(1), // Per minute
                        QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                        QueueLimit = 0                // Reject immediately if limit exceeded
                    }));

            // Optional: Custom response for rate limit exceeded
            options.OnRejected = async (context, cancellationToken) =>
            {
                context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                await context.HttpContext.Response.WriteAsync(
                    "Too many requests. Please try again later.", cancellationToken);
            };
        });

        return services;
    }

    public static IApplicationBuilder UseAuthDependencies(this IApplicationBuilder app)
    {
        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseRateLimiter();
        return app;
    }
}