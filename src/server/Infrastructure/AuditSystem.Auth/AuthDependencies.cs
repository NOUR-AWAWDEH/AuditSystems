using AuditSystem.Auth.Authentication;
using AuditSystem.Auth.Services.Account;
using AuditSystem.Auth.Services.Authentication;
using AuditSystem.Auth.Services.Email;
using AuditSystem.Auth.Services.Password.PasswordToken;
using AuditSystem.Auth.Services.Password.ResetPassword;
using AuditSystem.Auth.Services.Registration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using DotNetEnv;

namespace AuditSystem.Auth
{
    public static class AuthDependencies
    {
        public static IServiceCollection AddAuthDependencies(this IServiceCollection services)
        {
            // Load environment variables
            try
            {
                Env.Load(); // Must be early
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to load .env file", ex);
            }

            // Load JWT settings from env
            var jwtSettings = new JwtSettings
            {
                Key = Environment.GetEnvironmentVariable("JWT_KEY") ?? throw  new ArgumentNullException("JWT_KEY", "Missing JWT_KEY"),
                Issuer = Environment.GetEnvironmentVariable("JWT_ISSUER") ?? throw new  ArgumentNullException("JWT_ISSUER", "Missing JWT_ISSUER"),
                Audience = Environment.GetEnvironmentVariable("JWT_AUDIENCE") ?? throw new ArgumentNullException("JWT_AUDIENCE", "Missing JWT_AUDIENCE"),
                ExpirationMinutes = int.TryParse(Environment.GetEnvironmentVariable("JWT_EXPIRATION_MINUTES"), out int exp)
                    ? exp
                    : throw new ArgumentException("Invalid JWT_EXPIRATION_MINUTES")
            };

            services.AddSingleton(jwtSettings);

            // ✅ Explicitly configure JWT as the default authentication scheme
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
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
            });

            // Register core auth-related services
            services.AddScoped<ICustomAuthenticationService, CustomAuthenticationService>();
            services.AddScoped<IRegistrationService, RegistrationService>();
            services.AddScoped<IPasswordResetService, PasswordResetService>();
            services.AddScoped<IPasswordTokenService, PasswordTokenService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IAccountService, AccountService>();

            // SMTP
            ConfigureSmtpSettings(services);

            return services;
        }

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

            if (string.IsNullOrWhiteSpace(smtpConfig.SenderAddress) ||
                string.IsNullOrWhiteSpace(smtpConfig.UserName) ||
                string.IsNullOrWhiteSpace(smtpConfig.Password) ||
                string.IsNullOrWhiteSpace(smtpConfig.Host))
            {
                throw new InvalidOperationException("SMTP configuration is incomplete.");
            }

            services.AddSingleton<IOptions<SMTPConfigModel>>(Options.Create(smtpConfig));
        }
    }
}
