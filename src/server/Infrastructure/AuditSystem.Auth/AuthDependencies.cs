using AuditSystem.Auth.Authentication;
using AuditSystem.Auth.Services.Account;
using AuditSystem.Auth.Services.Authentication;
using AuditSystem.Auth.Services.Email;
using AuditSystem.Auth.Services.Password.PasswordToken;
using AuditSystem.Auth.Services.Password.ResetPassword;
using AuditSystem.Auth.Services.Registration;
using Microsoft.AspNetCore.Authentication.Cookies; // Added for cookie authentication
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http; // Added for StatusCodes
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;
using DotNetEnv; // Importing DotNetEnv for .env loading

namespace AuditSystem.Auth;

public static class AuthDependencies
{
    public static IServiceCollection AddAuthDependencies(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Load .env file using DotNetEnv (This should happen early in the program)
        Env.Load(); // Ensure this is done before accessing the configuration

        // JWT Settings
        var jwtSettings = new JwtSettings
        {
            Key = Environment.GetEnvironmentVariable("JWT_KEY") ?? throw new ArgumentNullException("JWT_KEY", "JWT_KEY is missing in configuration"),
            Issuer = Environment.GetEnvironmentVariable("JWT_ISSUER") ?? throw new ArgumentNullException("JWT_ISSUER", "JWT_ISSUER is missing in configuration"),
            Audience = Environment.GetEnvironmentVariable("JWT_AUDIENCE") ?? throw new ArgumentNullException("JWT_AUDIENCE", "JWT_AUDIENCE is missing in configuration"),
            ExpirationMinutes = int.TryParse(Environment.GetEnvironmentVariable("JWT_EXPIRATION_MINUTES"), out int expirationMinutes)
                ? expirationMinutes
                : throw new ArgumentException("Invalid or missing JWT_EXPIRATION_MINUTES in configuration", "JWT_EXPIRATION_MINUTES")
        };

        // Register JWT settings as singleton
        services.AddSingleton(jwtSettings);

        // Add Authentication services with both JWT and Cookie schemes
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme; // Default to JWT for APIs
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme; // Default challenge to JWT
            options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme; // Use cookies for web sign-in
        })
        .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
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
                ClockSkew = TimeSpan.Zero
            };
        })
        .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
        {
            // Prevent redirect to login page for APIs
            options.Events.OnRedirectToLogin = context =>
            {
                // Check if the request is for an API route (e.g., starts with /api)
                if (context.Request.Path.StartsWithSegments("/api"))
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    return Task.CompletedTask;
                }
                // Default redirect for non-API routes
                context.Response.Redirect(context.RedirectUri);
                return Task.CompletedTask;
            };

            options.Events.OnRedirectToAccessDenied = context =>
            {
                // Check if the request is for an API route
                if (context.Request.Path.StartsWithSegments("/api"))
                {
                    context.Response.StatusCode = StatusCodes.Status403Forbidden;
                    return Task.CompletedTask;
                }
                context.Response.Redirect(context.RedirectUri);
                return Task.CompletedTask;
            };
        });

        // Additional services for authentication
        services.AddScoped<ICustomAuthenticationService, CustomAuthenticationService>();
        services.AddScoped<IRegistrationService, RegistrationService>();
        services.AddScoped<IPasswordResetService, PasswordResetService>();
        services.AddScoped<IPasswordTokenService, PasswordTokenService>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<IAccountService, AccountService>();

        // Configure SMTP settings (to send emails)
        ConfigureSmtpSettings(services);

        return services;
    }

    // Configure SMTP settings from environment variables
    private static void ConfigureSmtpSettings(IServiceCollection services)
    {
        var smtpConfig = new SMTPConfigModel
        {
            SenderAddress = Environment.GetEnvironmentVariable("SMTP_SENDER_ADDRESS") ?? string.Empty,
            SenderDisplayName = Environment.GetEnvironmentVariable("SMTP_SENDER_NAME") ?? string.Empty,
            UserName = Environment.GetEnvironmentVariable("SMTP_USERNAME") ?? string.Empty,
            Password = Environment.GetEnvironmentVariable("SMTP_PASSWORD") ?? string.Empty,
            Host = Environment.GetEnvironmentVariable("SMTP_HOST") ?? string.Empty,
            Port = int.TryParse(Environment.GetEnvironmentVariable("SMTP_PORT"), out var port) ? port : 25,
            EnableSSL = bool.TryParse(Environment.GetEnvironmentVariable("SMTP_ENABLE_SSL"), out var ssl) && ssl,
            UseDefaultCredentials = false,
            IsBodyHtml = true
        };

        // Ensure the SMTP settings are correctly configured
        if (string.IsNullOrWhiteSpace(smtpConfig.SenderAddress) ||
            string.IsNullOrWhiteSpace(smtpConfig.UserName) ||
            string.IsNullOrWhiteSpace(smtpConfig.Password) ||
            string.IsNullOrWhiteSpace(smtpConfig.Host))
        {
            throw new InvalidOperationException("SMTP configuration is incomplete or missing environment variables.");
        }

        // Register SMTP settings as a singleton
        services.AddSingleton<IOptions<SMTPConfigModel>>(Options.Create(smtpConfig));
    }
}