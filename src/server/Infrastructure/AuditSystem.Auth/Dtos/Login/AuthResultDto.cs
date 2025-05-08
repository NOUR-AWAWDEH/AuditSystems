namespace AuditSystem.Auth.Dtos.Login;

public class AuthResultDto
{
    public string JwtToken { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
}