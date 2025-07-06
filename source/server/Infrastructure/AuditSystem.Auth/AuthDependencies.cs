using AuditSystem.Auth.Authentication;
using AuditSystem.Auth.Dtos.Register;
using AuditSystem.Auth.Services.Account;
using AuditSystem.Auth.Services.Authentication;
using AuditSystem.Auth.Services.Email;
using AuditSystem.Auth.Services.Password.PasswordToken;
using AuditSystem.Auth.Services.Password.ResetPassword;
using AuditSystem.Auth.Services.Registration;
using DotNetEnv;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace AuditSystem.Auth
{
    public static class AuthDependencies
    {
        public static IServiceCollection AddAuthDependencies(this IServiceCollection services)
        {
            try
            {
                Env.Load(); // Load .env variables
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to load .env file", ex);
            }

            // Load JWT settings
            var jwtSettings = new JwtSettings
            {
                Key = Environment.GetEnvironmentVariable("JWT_KEY") ?? throw new ArgumentNullException("JWT_KEY", "Missing JWT_KEY"),
                Issuer = Environment.GetEnvironmentVariable("JWT_ISSUER") ?? throw new ArgumentNullException("JWT_ISSUER", "Missing JWT_ISSUER"),
                Audience = Environment.GetEnvironmentVariable("JWT_AUDIENCE") ?? throw new ArgumentNullException("JWT_AUDIENCE", "Missing JWT_AUDIENCE"),
                ExpirationMinutes = int.TryParse(Environment.GetEnvironmentVariable("JWT_EXPIRATION_MINUTES"), out int exp)
                    ? exp
                    : throw new ArgumentException("Invalid JWT_EXPIRATION_MINUTES")
            };
            services.AddSingleton(jwtSettings);

            var domain = Environment.GetEnvironmentVariable("APP_DOMAIN") ?? throw new ArgumentNullException("APP_DOMAIN");
            services.Configure<AppSettings>(opt => opt.Domain = domain);


            // ✅ JWT Auth config
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

            // Auth-related services
            services.AddScoped<ICustomAuthenticationService, CustomAuthenticationService>();
            services.AddScoped<IRegistrationService, RegistrationService>();
            services.AddScoped<IPasswordResetService, PasswordResetService>();
            services.AddScoped<IPasswordTokenService, PasswordTokenService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IAccountService, AccountService>();

            services.Configure<SmtpSettings>(options =>
            {
                options.SenderAddress = Environment.GetEnvironmentVariable("SMTP_SENDER_ADDRESS") ?? throw new ArgumentNullException("SMTP_SENDER_ADDRESS");
                options.SenderName = Environment.GetEnvironmentVariable("SMTP_SENDER_NAME") ?? throw new ArgumentNullException("SMTP_SENDER_NAME");
                options.Password = Environment.GetEnvironmentVariable("SMTP_PASSWORD") ?? throw new ArgumentNullException("SMTP_PASSWORD");
                options.Host = Environment.GetEnvironmentVariable("SMTP_HOST") ?? throw new ArgumentNullException("SMTP_HOST");
                options.Port = int.TryParse(Environment.GetEnvironmentVariable("SMTP_PORT"), out int port)
                    ? port
                    : throw new ArgumentException("Invalid SMTP_PORT");
                options.EnableSsl = bool.TryParse(Environment.GetEnvironmentVariable("SMTP_ENABLE_SSL"), out bool enableSsl)
                    ? enableSsl
                    : throw new ArgumentException("Invalid SMTP_ENABLE_SSL");
            });

            return services;
        }

    }
}
