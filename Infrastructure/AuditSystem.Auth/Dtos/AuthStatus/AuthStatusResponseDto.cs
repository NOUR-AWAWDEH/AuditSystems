namespace AuditSystem.Auth.Dtos.AuthStatus;

public class AuthStatusResponseDto
{
    public bool IsAuthenticated { get; set; }
    public string? Username { get; set; }
    public string? AuthenticationType { get; set; }
    public string? Email { get; set; }
    public string? UserId { get; set; }
    public string? Roles { get; set; }
    public DateTimeOffset? ExpiresAtRaw { get; set; }
    public DateTimeOffset? AuthenticatedAtRaw { get; set; }
    public string? IpAddress { get; set; }
    public bool? IsEmailVerified { get; set; }

    // Make these settable properties instead of computed getters
    public string? ExpiresAt { get; set; }
    public string? AuthenticatedAt { get; set; }
}