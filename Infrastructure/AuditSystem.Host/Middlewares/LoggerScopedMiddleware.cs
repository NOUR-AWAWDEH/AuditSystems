namespace AuditSystem.Host.Middlewares;

public class LoggerScopedMiddleware(RequestDelegate next,
    ILogger<LoggerScopedMiddleware> logger)
{
    public async Task Invoke(HttpContext httpContext)
    {
        using (logger.BeginScope("\n\t"))
            await next(httpContext);
    }
}