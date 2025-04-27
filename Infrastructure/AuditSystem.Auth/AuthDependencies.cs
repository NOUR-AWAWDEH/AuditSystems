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
        services.AddMemoryCache();

        // Configure SMTP settings
        ConfigureSmtpSettings(services, configuration);

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
        services.Configure<SMTPConfigModel>(configuration.GetSection("SMTPConfig"));
    }
}
