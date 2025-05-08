using FluentValidation;
using System.Text.Json;
using AuditSystem.Contract.Common;

namespace AuditSystem.Host.Middlewares;

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
        _logger.LogError(ex, "An error occurred while processing request to {Path}: {Message}", context.Request.Path, ex.Message);

        if (context.Response.HasStarted)
        {
            _logger.LogWarning("Response has already started, cannot write error response.");
            return;
        }

        context.Response.ContentType = "application/json";

        ApiResponse response;

        if (ex is ValidationException validationEx)
        {
            var errors = validationEx.Errors
                .Select(e => new ApiErrorResponse($"{e.PropertyName}: {e.ErrorMessage}"));
            response = ApiResponse.BadRequest(errors);
            context.Response.StatusCode = response.StatusCode;

            _logger.LogWarning("Validation errors: {Errors}", JsonSerializer.Serialize(response.Errors));
            await WriteJsonResponse(context, response);
            return;
        }

        response = ex switch
        {
            KeyNotFoundException => ApiResponse.NotFound(new List<ApiErrorResponse> { new ApiErrorResponse(ex.Message) }),
            UnauthorizedAccessException => ApiResponse.Forbidden(new List<ApiErrorResponse> { new ApiErrorResponse(ex.Message) }),
            InvalidOperationException or ArgumentNullException or ArgumentException => ApiResponse.BadRequest(new List<ApiErrorResponse> { new ApiErrorResponse(ex.Message) }),
            _ => ApiResponse.InternalServerError(_env.IsDevelopment() ? ex.Message : "An unexpected error occurred.")
        };

 

        context.Response.StatusCode = response.StatusCode;
        await WriteJsonResponse(context, response);
    }

    private async Task WriteJsonResponse(HttpContext context, ApiResponse response)
    {
        try
        {
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
