namespace AuditSystem.Auth.Dtos.Password;

public class ForgotPasswordRequest
{
    public required string Email { get; set; } = string.Empty;
}
