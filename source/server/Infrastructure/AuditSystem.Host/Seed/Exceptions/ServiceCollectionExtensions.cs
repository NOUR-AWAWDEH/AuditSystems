using AuditSystem.Host.Seed.Interfaces;
using AuditSystem.Host.Seed.Services;

namespace AuditSystem.Host.Seed.Exceptions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAdminSeeding(this IServiceCollection services)
    {
        services.AddScoped<IAdminSeedingOrchestrator, AdminSeedingOrchestrator>();
        return services;
    }
}