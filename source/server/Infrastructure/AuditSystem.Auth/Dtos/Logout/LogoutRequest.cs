namespace AuditSystem.Auth.Dtos.Logout;

public class LogoutRequest
{
    public string RefreshToken { get; set; } = string.Empty;
}