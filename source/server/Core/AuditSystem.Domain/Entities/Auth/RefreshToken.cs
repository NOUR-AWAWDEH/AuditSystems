using AuditSystem.Domain.Entities.Common;
using AuditSystem.Domain.Entities.Users;


namespace AuditSystem.Domain.Entities.Auth;

public class RefreshToken : Entity<Guid>
{
    public required string Token { get; set; } = string.Empty;
    public Guid UserId { get; set; }
    public required DateTime ExpiresAt { get; set; }
    public required bool IsRevoked { get; set; }
    public new DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public virtual User User { get; set; } = null!;
}