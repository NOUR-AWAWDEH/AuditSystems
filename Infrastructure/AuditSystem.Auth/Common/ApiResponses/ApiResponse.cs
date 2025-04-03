namespace AuditSystem.Auth.Common.ApiResponses;

public class ApiResponse<T>
{
    public bool Success { get; }
    public string Message { get; }
    public T? Data { get; }
    public IReadOnlyList<ErrorDetail>? Errors { get; }

    protected ApiResponse(bool success, string message, T? data, IEnumerable<ErrorDetail>? errors)
    {
        Success = success;
        Message = message ?? string.Empty;
        Data = data;
        Errors = errors?.ToList().AsReadOnly();
    }

    // Success factory methods
    public static ApiResponse<T> SuccessResponse(T data, string message = "Operation completed successfully")
    {
        ArgumentNullException.ThrowIfNull(data, nameof(data));
        return new ApiResponse<T>(true, message, data, null);
    }

    public static ApiResponse<T> SuccessResponse(string message = "Operation completed successfully")
        => new ApiResponse<T>(true, message, default, null);

    // Error factory methods
    public static ApiResponse<T> ErrorResponse(string message, IEnumerable<ErrorDetail>? errors = null)
        => new ApiResponse<T>(false, message, default, errors);

    public static ApiResponse<T> ErrorResponse(string message, string errorCode, string errorDetail)
        => new ApiResponse<T>(false, message, default, new[] { new ErrorDetail(errorCode, errorDetail) });

    public static ApiResponse<T> NotFound(string entityName)
    {
        if (string.IsNullOrEmpty(entityName))
            throw new ArgumentException("Entity name cannot be null or empty", nameof(entityName));

        return new ApiResponse<T>(
            success: false,
            message: $"{entityName} not found",
            data: default,
            errors: new[] { new ErrorDetail("NOT_FOUND", $"{entityName} could not be located in the system") }
        );
    }
}

// Non-generic version for responses without data
public class ApiResponse : ApiResponse<object>
{
    private ApiResponse(bool success, string message, IEnumerable<ErrorDetail>? errors)
        : base(success, message, null, errors)
    {
    }

    // Success factory methods
    public static ApiResponse SuccessResponse(string message = "Operation completed successfully")
        => new ApiResponse(true, message, null);

    // Error factory methods
    public static ApiResponse ErrorResponse(string message, IEnumerable<ErrorDetail>? errors = null)
        => new ApiResponse(false, message, errors);

    public static ApiResponse ErrorResponse(string message, string errorCode, string errorDetail)
        => new ApiResponse(false, message, new[] { new ErrorDetail(errorCode, errorDetail) });

    // Common error scenarios
    public static ApiResponse ValidationError(IEnumerable<ValidationError> errors)
    {
        ArgumentNullException.ThrowIfNull(errors, nameof(errors));
        return new ApiResponse(false, "Validation failed", errors.Select(e =>
            new ErrorDetail(e.ErrorCode, e.ErrorMessage)));
    }

    public static ApiResponse NotFound(string entityName)
        => new ApiResponse(false, $"{entityName} not found",
            new[] { new ErrorDetail("NOT_FOUND", $"{entityName} could not be located") });

    public static ApiResponse Unauthorized(string message = "Unauthorized access")
        => new ApiResponse(false, message,
            new[] { new ErrorDetail("UNAUTHORIZED", "Access denied due to insufficient permissions") });
}
