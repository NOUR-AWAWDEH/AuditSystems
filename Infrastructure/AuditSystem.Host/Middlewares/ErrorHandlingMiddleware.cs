using Newtonsoft.Json;
using System.Net.Mime;

namespace AuditSystem.Host.Middlewares;
public class ErrorHandlingMiddleware
{
    public ErrorHandlingMiddleware(RequestDelegate next,
    ILogger<ErrorHandlingMiddleware> logger,
    IHostEnvironment hostEnvironment)
    {
        Next = next;
        Logger = logger;
        HostEnvironment = hostEnvironment;
    }

    private RequestDelegate Next { get; }
    private ILogger<ErrorHandlingMiddleware> Logger { get; }
    private IHostEnvironment HostEnvironment { get; }

    public async Task Invoke(HttpContext httpContext)
    {
        try
        {
            await Next(httpContext);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "An unexpected exception was thrown: {Message}", ex.Message);

            httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
            httpContext.Response.ContentType = MediaTypeNames.Application.Json;
            var serializedException = JsonConvert.SerializeObject(ex);
            await httpContext.Response.WriteAsync(serializedException);
        }
    }
}
