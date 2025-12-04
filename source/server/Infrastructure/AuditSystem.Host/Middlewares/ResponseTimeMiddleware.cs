using System.Diagnostics;

namespace AuditSystem.Host.Middlewares;

public class ResponseTimeMiddleware
{
    private readonly RequestDelegate _next;

    public ResponseTimeMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var stopwatch = Stopwatch.StartNew();

        // Register callback to add header before response starts
        context.Response.OnStarting(() =>
        {
            stopwatch.Stop();
            try
            {
                context.Response.Headers.TryAdd("Response-Time", $"{stopwatch.ElapsedMilliseconds}ms");
            }
            catch
            {
                // Silently ignore if headers are already sent
            }
            return Task.CompletedTask;
        });

        // Call the next middleware in the pipeline
        await _next(context);
    }
}