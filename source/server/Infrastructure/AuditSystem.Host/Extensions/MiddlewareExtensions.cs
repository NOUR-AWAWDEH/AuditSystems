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


    public static IApplicationBuilder UseResponseTime(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ResponseTimeMiddleware>();
    }

    // Recommended extension method for proper middleware ordering
    public static IApplicationBuilder UseCustomMiddlewarePipeline(this IApplicationBuilder builder)
    {
        // Recommended order:
        return builder
            .UseResponseTime()         // First - track response time
            .UseScopedLogging()        // Second - setup request logging
            .UseValidation()           // Third - handle validation
            .UseErrorHandling();      // Last - catch all exceptions
    }
}