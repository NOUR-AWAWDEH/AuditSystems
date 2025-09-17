using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http;

namespace AuditSystem.Contract.Common;

public class ApiResponse
{
    [JsonConstructor]
    public ApiResponse(bool success, string successMessage, int statusCode, IEnumerable<ApiErrorResponse> errors)
    {
        Success = success;
        SuccessMessage = successMessage;
        StatusCode = statusCode;
        Errors = errors;
    }

    public ApiResponse() { }

    public bool Success { get; protected init; }
    public string? SuccessMessage { get; protected init; }
    public int StatusCode { get; protected init; }
    public IEnumerable<ApiErrorResponse> Errors { get; protected init; } = Enumerable.Empty<ApiErrorResponse>();

    public static ApiResponse Ok() =>
        new() { Success = true, StatusCode = StatusCodes.Status200OK };

    public static ApiResponse Ok(string successMessage) =>
        new() { Success = true, StatusCode = StatusCodes.Status200OK, SuccessMessage = successMessage };

    public static ApiResponse BadRequest(string errorMessage) =>
        new() { Success = false, StatusCode = StatusCodes.Status400BadRequest, Errors = CreateErrors(errorMessage) };

    public static ApiResponse BadRequest(IEnumerable<ApiErrorResponse> errors) =>
        new() { Success = false, StatusCode = StatusCodes.Status400BadRequest, Errors = errors };

    public static ApiResponse Unauthorized(string errorMessage) =>
        new() { Success = false, StatusCode = StatusCodes.Status401Unauthorized, Errors = CreateErrors(errorMessage) };

    public static ApiResponse Unauthorized(IEnumerable<ApiErrorResponse> errors) =>
        new() { Success = false, StatusCode = StatusCodes.Status401Unauthorized, Errors = errors };

    public static ApiResponse Forbidden(string errorMessage) =>
        new() { Success = false, StatusCode = StatusCodes.Status403Forbidden, Errors = CreateErrors(errorMessage) };
    
    public static ApiResponse Forbidden(IEnumerable<ApiErrorResponse> errors) =>
    new()
    {
        Success = false,
        StatusCode = StatusCodes.Status403Forbidden,
        Errors = errors
    };
    public static ApiResponse NotFound(IEnumerable<ApiErrorResponse> errors) =>
    new() { Success = false, StatusCode = StatusCodes.Status404NotFound, Errors = errors };

    public static ApiResponse InternalServerError(string errorMessage) =>
        new() { Success = false, StatusCode = StatusCodes.Status500InternalServerError, Errors = CreateErrors(errorMessage) };

    public static ApiResponse InternalServerError(IEnumerable<ApiErrorResponse> errors) =>
        new() { Success = false, StatusCode = StatusCodes.Status500InternalServerError, Errors = errors };

    protected static ApiErrorResponse[] CreateErrors(string errorMessage) =>
        new[] { new ApiErrorResponse(errorMessage) };

    public override string ToString() =>
        $"Success: {Success} | StatusCode: {StatusCode} | HasErrors: {Errors.Any()}";
}

public sealed class ApiResponse<T> : ApiResponse
{
    [JsonConstructor]
    public ApiResponse(T result, bool success, string successMessage, int statusCode, IEnumerable<ApiErrorResponse> errors)
        : base(success, successMessage, statusCode, errors)
    {
        Result = result;
    }

    public ApiResponse() { }

    public T? Result { get; private init; }

    public static ApiResponse<T> Ok(T result) =>
        new() { Success = true, StatusCode = StatusCodes.Status200OK, Result = result };

    public static ApiResponse<T> Ok(T result, string successMessage) =>
        new() { Success = true, StatusCode = StatusCodes.Status200OK, SuccessMessage = successMessage, Result = result };

    public static ApiResponse<T> Created(T result) =>
        new() { Success = true, StatusCode = StatusCodes.Status201Created, Result = result };

    public static new ApiResponse<T> BadRequest(string errorMessage) =>
        new()
        {
            Success = false,
            StatusCode = StatusCodes.Status400BadRequest,
            Errors = CreateErrors(errorMessage),
            Result = default
        };

    public static new ApiResponse<T> BadRequest(IEnumerable<ApiErrorResponse> errors) =>
        new()
        {
            Success = false,
            StatusCode = StatusCodes.Status400BadRequest,
            Errors = errors,
            Result = default
        };

    public static new ApiResponse<T> Unauthorized(string errorMessage) =>
        new()
        {
            Success = false,
            StatusCode = StatusCodes.Status401Unauthorized,
            Errors = CreateErrors(errorMessage),
            Result = default
        };

    public static new ApiResponse<T> Forbidden(string errorMessage) =>
        new()
        {
            Success = false,
            StatusCode = StatusCodes.Status403Forbidden,
            Errors = CreateErrors(errorMessage),
            Result = default
        };

    public static ApiResponse<T> NotFound(string errorMessage) =>
        new()
        {
            Success = false,
            StatusCode = StatusCodes.Status404NotFound,
            Errors = CreateErrors(errorMessage),
            Result = default
        };

    public static new ApiResponse<T> InternalServerError(string errorMessage) =>
        new()
        {
            Success = false,
            StatusCode = StatusCodes.Status500InternalServerError,
            Errors = CreateErrors(errorMessage),
            Result = default
        };

    public static new ApiResponse<T> InternalServerError(IEnumerable<ApiErrorResponse> errors) =>
        new()
        {
            Success = false,
            StatusCode = StatusCodes.Status500InternalServerError,
            Errors = errors,
            Result = default
        };
}

public sealed record ApiErrorResponse(string Message);
