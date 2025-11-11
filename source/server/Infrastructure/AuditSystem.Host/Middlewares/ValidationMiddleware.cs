using FluentValidation;

namespace AuditSystem.Host.Middlewares;

internal sealed class ValidationMiddleware
{
    public ValidationMiddleware(RequestDelegate next,
        ILogger<ValidationMiddleware> logger)
    {
        Next = next;
        Logger = logger;
    }

    private RequestDelegate Next { get; }

    private ILogger<ValidationMiddleware> Logger { get; }

    public async Task Invoke(HttpContext httpContext)
    {
        try
        {
            await Next(httpContext);
        }
        catch (ValidationException ex)
        {
            Logger.LogError(ex, "Validation exception. Message: {Message}.", ex.Message);
            throw;
        }
    }
}