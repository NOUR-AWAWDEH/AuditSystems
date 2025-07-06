namespace AuditSystem.Auth.Dtos.Login;

public class LoginResponseDto
{
    public string JwtToken { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
    public bool IsEmailVerified { get; set; } = false;
}
