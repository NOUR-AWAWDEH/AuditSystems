namespace AuditSystem.Auth.Common.ApiResponses;

// Specific validation error class
public class ValidationError
{
    public string ErrorCode { get; }
    public string ErrorMessage { get; }
    public string? PropertyName { get; }

    public ValidationError(string errorCode, string errorMessage, string? propertyName = null)
    {
        ErrorCode = errorCode;
        ErrorMessage = errorMessage;
        PropertyName = propertyName;
    }
}