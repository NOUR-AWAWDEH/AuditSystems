using AuditSystem.Contract.Interfaces.Cache;
using AuditSystem.Host.Caching;

namespace AuditSystem.Host.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddClients(this IServiceCollection services, IConfiguration configuration, bool isProduction)
    {
        services.AddScoped<ICacheService, CacheService>();
        services.AddCaching();

        return services;
    }
}