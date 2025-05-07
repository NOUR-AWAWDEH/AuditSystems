using AuditSystem.Auth.Authentication;
using AuditSystem.Auth.Services.Account;
using AuditSystem.Auth.Services.Authentication;
using AuditSystem.Auth.Services.Email;
using AuditSystem.Auth.Services.Password.PasswordToken;
using AuditSystem.Auth.Services.Password.ResetPassword;
using AuditSystem.Auth.Services.Registration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;

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
        services.AddMemoryCache();

        // Configure SMTP settings
        ConfigureSmtpSettings(services, configuration);

        // JWT Settings
        var jwtSettings = new JwtSettings
        {
            Key = configuration["JWT_KEY"] ?? throw new ArgumentNullException("JWT_KEY", "JWT_KEY is missing in configuration"),
            Issuer = configuration["JWT_ISSUER"] ?? throw new ArgumentNullException("JWT_ISSUER", "JWT_ISSUER is missing in configuration"),
            Audience = configuration["JWT_AUDIENCE"] ?? throw new ArgumentNullException("JWT_AUDIENCE", "JWT_AUDIENCE is missing in configuration"),
            ExpirationMinutes = int.TryParse(configuration["JWT_EXPIRATION_MINUTES"], out int expirationMinutes)
        ? expirationMinutes
        : throw new ArgumentException("Invalid or missing JWT_EXPIRATION_MINUTES in configuration", "JWT_EXPIRATION_MINUTES")
        };

        services.AddSingleton(jwtSettings);

        // Fix: Use AddAuthentication to get an AuthenticationBuilder, then call AddJwtBearer
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
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
            });

        return services;
    }

    private static void ConfigureSmtpSettings(IServiceCollection services, IConfiguration configuration)
    {
        var smtpConfig = new SMTPConfigModel
        {
            SenderAddress = Environment.GetEnvironmentVariable("SMTP_SENDER_ADDRESS") ?? "",
            SenderDisplayName = Environment.GetEnvironmentVariable("SMTP_SENDER_NAME") ?? "",
            UserName = Environment.GetEnvironmentVariable("SMTP_USERNAME") ?? "",
            Password = Environment.GetEnvironmentVariable("SMTP_PASSWORD") ?? "",
            Host = Environment.GetEnvironmentVariable("SMTP_HOST") ?? "",
            Port = int.TryParse(Environment.GetEnvironmentVariable("SMTP_PORT"), out var port) ? port : 25,
            EnableSSL = bool.TryParse(Environment.GetEnvironmentVariable("SMTP_ENABLE_SSL"), out var ssl) && ssl,
            UseDefaultCredentials = false,
            IsBodyHtml = true
        };

        services.AddSingleton<IOptions<SMTPConfigModel>>(Options.Create(smtpConfig));
    }
}
