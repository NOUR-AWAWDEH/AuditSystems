using FluentValidation;
using System.Text.Json;

namespace AuditSystem.Host.Middlewares
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;
        private readonly IWebHostEnvironment _env;

        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger, IWebHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            // Log the exception with additional context
            _logger.LogError(ex, "An error occurred while processing request to {Path}: {Message}", context.Request.Path, ex.Message);

            // Prevent writing to response if it has already started
            if (context.Response.HasStarted)
            {
                _logger.LogWarning("Response has already started, cannot write error response.");
                return;
            }

            // Set content type
            context.Response.ContentType = "application/json";

            // Handle ValidationException
            if (ex is ValidationException validationEx)
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;

                var errorResponse = new
                {
                    Status = context.Response.StatusCode,
                    Message = "Validation failed.",
                    Errors = validationEx.Errors.Select(e => new
                    {
                        e.PropertyName,
                        e.ErrorMessage,
                        e.Severity
                    }).ToList()
                };

                // Log validation errors specifically
                _logger.LogWarning("Validation errors: {Errors}", JsonSerializer.Serialize(errorResponse.Errors));

                await WriteJsonResponse(context, errorResponse);
                return;
            }

            // Handle other exceptions
            context.Response.StatusCode = ex switch
            {
                KeyNotFoundException => StatusCodes.Status404NotFound,
                UnauthorizedAccessException => StatusCodes.Status403Forbidden,
                InvalidOperationException => StatusCodes.Status400BadRequest,
                ArgumentNullException => StatusCodes.Status400BadRequest,
                ArgumentException => StatusCodes.Status400BadRequest,
                _ => StatusCodes.Status500InternalServerError
            };

            var generalErrorResponse = new
            {
                Status = context.Response.StatusCode,
                Message = _env.IsDevelopment() ? ex.Message : "An unexpected error occurred.",
                Details = _env.IsDevelopment() ? ex.StackTrace : null
            };

            await WriteJsonResponse(context, generalErrorResponse);
        }

        private async Task WriteJsonResponse(HttpContext context, object response)
        {
            try
            {
                // Use System.Text.Json for serialization to avoid Newtonsoft.Json issues
                var json = JsonSerializer.Serialize(response, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    WriteIndented = _env.IsDevelopment()
                });
                await context.Response.WriteAsync(json);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to serialize error response.");
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                await context.Response.WriteAsync("{\"status\":500,\"message\":\"Failed to process error response.\"}");
            }
        }
    }
}