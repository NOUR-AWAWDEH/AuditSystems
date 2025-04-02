namespace AuditSystem.Auth.Dtos
{
    public class AuthStatusResponseDto
    {

        public bool IsAuthenticated { get; set; }

        public string? Username { get; set; }

        public string? AuthenticationType { get; set; }

        public string? Email { get; set; }

        public string? UserId { get; set; }

        public string? Roles { get; set; }

        public DateTimeOffset? ExpiresAt { get; set; }

        public DateTimeOffset? AuthenticatedAt { get; set; }

        public string? IpAddress { get; set; }

        public Dictionary<string, string>? Claims { get; set; } = new Dictionary<string, string>();

        public bool? IsEmailVerified { get; set; }

        public bool? IsMfaAuthenticated { get; set; }

    }
}
