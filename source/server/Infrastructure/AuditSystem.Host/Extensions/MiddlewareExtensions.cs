using AuditSystem.Host.Middlewares;

namespace AuditSystem.Host.Extensions;

public static class MiddlewareExtensions
{
    public static IApplicationBuilder UseScopedLogging(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<LoggerScopedMiddleware>();
    }

    public static IApplicationBuilder UseValidation(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ValidationMiddleware>();
    }

    public static IApplicationBuilder UseErrorHandling(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ErrorHandlingMiddleware>();
    }

    // Recommended extension method for proper middleware ordering
    public static IApplicationBuilder UseCustomMiddlewarePipeline(this IApplicationBuilder builder)
    {
        // Recommended order:
        return builder
            .UseScopedLogging()        // First - setup request logging
            .UseValidation()           // Second - handle validation
            .UseErrorHandling();      // Last - catch all exceptions
    }
}