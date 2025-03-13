using AuditSystem.Contract.Interfaces.Contexts;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.DataAccess.Constants;
using AuditSystem.DataAccess.Contexts;
using AuditSystem.DataAccess.Repositories;
using AuditSystem.Domain.Entities.Common;
using AuditSystem.Domain.Entities.Risks;
using AuditSystem.Domain.Entities.Users; // Ensure User and Role are imported
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AuditSystem.DataAccess;

public static class InfrastructureDependencies
{
    public static IServiceCollection AddDataAccess(this IServiceCollection services,
        IConfiguration configuration,
        IHealthChecksBuilder healthChecksBuilder)
    {
        services
            .AddDbContext(configuration)
            .AddRepositories()
            .AddIdentity();
        healthChecksBuilder.AddDbContextHealthCheck();
        return services;
    }

    private static IServiceCollection AddIdentity(this IServiceCollection services)
    {
        services.AddIdentity<User, Role>(options =>
        {
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireUppercase = true;
            options.Password.RequireNonAlphanumeric = true;
            options.Password.RequiredLength = 8;
            options.User.RequireUniqueEmail = true;
        })
        .AddEntityFrameworkStores<ApplicationDbContext>()
        .AddDefaultTokenProviders();

        return services;
    }

    private static IServiceCollection AddDbContext(this IServiceCollection services, IConfiguration configuration) =>
        services
            .AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString(ConfigurationKeys.DefaultConnectionString)))
            .AddScoped<IContext, EntityContext>();

    private static IServiceCollection AddRepositories(this IServiceCollection services) =>
        services
            .AddRepository<Guid, Risk>()
            // Add more repositories as needed, e.g.:
            .AddRepository<Guid, UserManagement>()
            .AddRepository<Guid, AuditorSettings>()
            .AddRepository<Guid, Skill>()
            .AddRepository<Guid, SkillSet>();

    private static IServiceCollection AddRepository<TId, TEntity>(this IServiceCollection services)
        where TId : IComparable<TId>
        where TEntity : Entity<TId>
    {
        services.AddScoped<IRepository<TId, TEntity>, EntityRepository<TId, TEntity>>();
        return services;
    }

    private static IHealthChecksBuilder AddDbContextHealthCheck(this IHealthChecksBuilder healthChecksBuilder) =>
        healthChecksBuilder.AddDbContextCheck<ApplicationDbContext>(
            name: "ApplicationDbContext-HealthCheck",
            tags: new[] { "db", "sqlserver" });
}