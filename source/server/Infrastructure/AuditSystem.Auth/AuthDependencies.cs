using AuditSystem.Auth.Authentication;
using AuditSystem.Contract.Dtos.Register;
using AuditSystem.Auth.Services.Account;
using AuditSystem.Auth.Services.Authentication;
using AuditSystem.Auth.Services.Email.Abstractions;
using AuditSystem.Auth.Services.Email.Services;
using AuditSystem.Auth.Services.Email.Services.Renderers;
using AuditSystem.Auth.Services.Email.Services.Smtp;
using AuditSystem.Auth.Services.Password.PasswordToken;
using AuditSystem.Auth.Services.Password.ResetPassword;
using AuditSystem.Auth.Services.Registration;
using DotNetEnv;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using AuditSystem.Domain.Entities.Users;
using Microsoft.AspNetCore.Identity;
using AuditSystem.Auth.Services.Email.Models;

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

                options.Events = new JwtBearerEvents
                {
                    OnTokenValidated = async context =>
                    {
                        var userManager = context.HttpContext.RequestServices.GetRequiredService<UserManager<ApplicationUser>>();
                        var userId = context.Principal?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                        if (string.IsNullOrEmpty(userId))
                        {
                            context.Fail("Unauthorized");
                            return;
                        }

                        var user = await userManager.FindByIdAsync(userId);
                        if (user == null)
                        {
                            context.Fail("Unauthorized");
                            return;
                        }

                        // Check security stamp to ensure the user has not changed their password or logout from another device
                        var tokenSecurityStamp = context.Principal?.FindFirst("AspNet.Identity.SecurityStamp")?.Value;
                        var userSecurityStamp = await userManager.GetSecurityStampAsync(user);

                        if (tokenSecurityStamp != userSecurityStamp)
                        {
                            context.Fail("Unauthorized");
                        }
                    }
                };
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("Admin", policy => policy.RequireRole("Admin"));
            });

            // Auth-related services
            services.AddScoped<ICustomAuthenticationService, CustomAuthenticationService>();
            services.AddScoped<IRegistrationService, RegistrationService>();
            services.AddScoped<IPasswordResetService, PasswordResetService>();
            services.AddScoped<IPasswordTokenService, PasswordTokenService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<ISmtpClientFactory, SmtpClientFactory>();
            services.AddScoped<IEmailTemplateRenderer, EmbeddedEmailTemplateRenderer>();
            services.AddScoped<IEmailSender, SmtpEmailSender>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IAccountService, AccountService>();

            _ = services.Configure<SmtpSettings>(options =>
            {
                options.SenderAddress = Environment.GetEnvironmentVariable("SMTP_SENDER_ADDRESS") ?? throw new ArgumentNullException("SMTP_SENDER_ADDRESS");
                options.SenderName = Environment.GetEnvironmentVariable("SMTP_SENDER_NAME") ?? throw new ArgumentNullException("SMTP_SENDER_NAME");
                options.Password = Environment.GetEnvironmentVariable("SMTP_PASSWORD") ?? throw new ArgumentNullException("SMTP_PASSWORD");
                options.Host = Environment.GetEnvironmentVariable("SMTP_HOST") ?? throw new ArgumentNullException("services");
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