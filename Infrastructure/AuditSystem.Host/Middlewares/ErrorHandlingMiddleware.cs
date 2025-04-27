using FluentValidation;
using Newtonsoft.Json;
using System.Net.Mime;

namespace AuditSystem.Host.Middlewares;

public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorHandlingMiddleware> _logger;
    private readonly IHostEnvironment _hostEnvironment;

    public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger, IHostEnvironment hostEnvironment)
    {
        _next = next;
        _logger = logger;
        _hostEnvironment = hostEnvironment;
    }

    public async Task Invoke(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception ex)
        {
            // Log the error
            _logger.LogError(ex, "An unexpected exception was thrown: {Message}", ex.Message);

            // Set response status code based on exception type
            httpContext.Response.StatusCode = ex switch
            {
                KeyNotFoundException => StatusCodes.Status404NotFound,
                UnauthorizedAccessException => StatusCodes.Status404NotFound,
                InvalidOperationException => StatusCodes.Status400BadRequest,
                ArgumentNullException => StatusCodes.Status400BadRequest,
                ArgumentException => StatusCodes.Status400BadRequest,
                ValidationException => StatusCodes.Status400BadRequest,
                _ => StatusCodes.Status500InternalServerError
            };

            // Set JSON response content type
            httpContext.Response.ContentType = MediaTypeNames.Application.Json;

            // Create structured error response
            var errorResponse = new
            {
                status = httpContext.Response.StatusCode,
                message = _hostEnvironment.IsDevelopment() ? ex.Message : "An unexpected error occurred.",
                details = _hostEnvironment.IsDevelopment() ? ex.StackTrace : null
            };

            var serialized = JsonConvert.SerializeObject(errorResponse);
            await httpContext.Response.WriteAsync(serialized);
        }
    }
}