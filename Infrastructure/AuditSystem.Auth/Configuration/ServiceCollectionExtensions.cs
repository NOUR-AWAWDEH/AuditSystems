using AuditSystem.Auth.Authentication;
using AuditSystem.Auth.Data;
using AuditSystem.Auth.Models;
using AuditSystem.Auth.Services.Account;
using AuditSystem.Auth.Services.Authentication;
using AuditSystem.Auth.Services.Email;
using AuditSystem.Auth.Services.Password.PasswordToken;
using AuditSystem.Auth.Services.Password.ResetPassword;
using AuditSystem.Auth.Services.Registration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Threading.RateLimiting;

namespace AuditSystem.Auth.Configuration
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<ICustomAuthenticationService, CustomAuthenticationService>();
            services.AddScoped<IRegistrationService, RegistrationService>();
            services.AddScoped<IPasswordResetService, PasswordResetService>();
            services.AddScoped<IPasswordTokenService, PasswordTokenService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IEmailService, EmailService>();

            services.AddScoped<IAccountService, AccountService>();

            services.AddScoped<ApplicationDbContext>();
            services.AddScoped<UserManager<ApplicationUser>>();

            return services;
        }

        public static IServiceCollection AddApplicationDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
            return services;
        }

        public static IServiceCollection AddApplicationIdentity(this IServiceCollection services)
        {
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();
            return services;
        }

        public static IServiceCollection AddApplicationAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtSettings = new JwtSettings
            {
                Key = configuration["JwtSettings:Key"] ?? throw new ArgumentNullException("JwtSettings:Key is missing in configuration"),
                Issuer = configuration["JwtSettings:Issuer"] ?? throw new ArgumentNullException("JwtSettings:Issuer is missing in configuration"),
                Audience = configuration["JwtSettings:Audience"] ?? throw new ArgumentNullException("JwtSettings:Audience is missing in configuration"),
                ExpirationMinutes = configuration["JwtSettings:ExpirationMinutes"] ?? throw new ArgumentException("Invalid ExpirationMinutes")
            };

            configuration.GetSection("JwtSettings").Bind(jwtSettings);

            services.AddSingleton(jwtSettings);
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
                    ValidateLifetime = true
                };
            });

            return services;
        }

        public static IServiceCollection AddApplicationRateLimiting(this IServiceCollection services)
        {
            services.AddRateLimiter(options =>
            {
                options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
                options.AddPolicy("loginPolicy", httpContent =>
                RateLimitPartition.GetFixedWindowLimiter(
                    partitionKey: httpContent.Connection.RemoteIpAddress?
                    .ToString() ?? "Unknown",
                    factory: PartitionedRateLimiter => new FixedWindowRateLimiterOptions
                    {
                        PermitLimit = 8,
                        Window = TimeSpan.FromMinutes(1)
                    }));
            });

            return services;
        }
    }
}
