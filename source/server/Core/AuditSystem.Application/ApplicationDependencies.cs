using AuditSystem.Application.Behaviors;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace AuditSystem.Application;

[ExcludeFromCodeCoverage]
public static class ApplicationDependencies
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();

        services.AddValidatorsFromAssembly(assembly, includeInternalTypes: true)
                .AddMediatR(assembly)
                .AddAutoMapper(assembly)
                .AddApplicationBehaviors();

        return services;
    }

    private static IServiceCollection AddMediatR(this IServiceCollection services, Assembly assembly) =>
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(assembly));

    private static IServiceCollection AddApplicationBehaviors(this IServiceCollection services) =>
        services
            .AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>))
            .AddTransient(typeof(IPipelineBehavior<,>), typeof(MediatRLoggingBehavior<,>))
            .AddTransient(typeof(IPipelineBehavior<,>), typeof(CommandTransactionBehavior<,>));
}