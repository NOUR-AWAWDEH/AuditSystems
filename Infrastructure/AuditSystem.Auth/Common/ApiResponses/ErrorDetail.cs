namespace AuditSystem.Auth.Common.ApiResponses;

// Error detail class to provide structured error information
public class ErrorDetail
{
    public string Code { get; }
    public string Detail { get; }

    public ErrorDetail(string code, string detail)
    {
        Code = code ?? throw new ArgumentNullException(nameof(code));
        Detail = detail ?? throw new ArgumentNullException(nameof(detail));
    }
}
