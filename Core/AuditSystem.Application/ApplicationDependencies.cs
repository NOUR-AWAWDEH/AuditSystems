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
        var assembly = Assembly.GetAssembly(typeof(ApplicationDependencies))!;

        services.AddValidatorsFromAssembly(assembly)
            .AddMediatR(assembly)
            .AddMapping();

        return services;
    }

    private static IServiceCollection AddMediatR(this IServiceCollection services, Assembly assembly) =>
        services
            .AddMediatR(cfg =>
                cfg.RegisterServicesFromAssembly(assembly)
                   .AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>))
                   .AddBehavior(typeof(IPipelineBehavior<,>), typeof(MediatRLoggingBehavior<,>))
                   .AddBehavior(typeof(IPipelineBehavior<,>), typeof(CommandTransactionBehavior<,>)));

    private static IServiceCollection AddMapping(this IServiceCollection services) =>
        services.AddAutoMapper(cfg => cfg.AddMaps(typeof(ApplicationDependencies).Assembly));
}