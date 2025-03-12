using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.DataAccess.Constants;
using AuditSystem.DataAccess.Contexts;
using AuditSystem.DataAccess.Repositories;
using AuditSystem.Domain.Entities.Common;
using AuditSystem.Domain.Entities.Risks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using AuditSystem.Contract.Interfaces.Contexts;

namespace AuditSystem.DataAccess;

public static class InfrastructureDependencies
{
    public static IServiceCollection AddDataAccess(this IServiceCollection services,
        IConfiguration configuration,
        IHealthChecksBuilder healthChecksBuilder)
    {
        services
        .AddDbContext(configuration)
        .AddRepositories();

        healthChecksBuilder.AddDbContextHealthCheck();
        return services;
    }

    private static IServiceCollection AddDbContext(this IServiceCollection services, IConfiguration configuration) =>
    services
        .AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString(ConfigurationKeys.DefaultConnectionString)))
        .AddScoped<IContext, EntityContext>();

    private static IServiceCollection AddRepositories(this IServiceCollection services) =>
    services
        .AddRepository<Guid, Risk>();

    private static IServiceCollection AddRepository<TId, TEntity>(this IServiceCollection services)
        where TId : IComparable<TId>
        where TEntity : Entity<TId>
    {
        services.AddScoped<IRepository<TId, TEntity>, EntityRepository<TId, TEntity>>();
        return services;
    }

    private static IHealthChecksBuilder AddDbContextHealthCheck(this IHealthChecksBuilder healthChecksBuilder) =>
        healthChecksBuilder.AddDbContextCheck<ApplicationDbContext>(nameof(ApplicationDbContext));

}
