using AuditSystem.Host.Middlewares;

namespace AuditSystem.Host.Extensions;

public static class MiddlewareExtensions
{
    public static void UseErrorHandling(this IApplicationBuilder builder) =>
        builder.UseMiddleware<ErrorHandlingMiddleware>();

    public static void UseValidation(this IApplicationBuilder builder) =>
        builder.UseMiddleware<ValidationMiddleware>();

    public static void UseScopedLogging(this IApplicationBuilder builder) =>
        builder.UseMiddleware<LoggerScopedMiddleware>();
}