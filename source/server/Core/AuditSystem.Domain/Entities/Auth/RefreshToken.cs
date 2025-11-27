using AuditSystem.Domain.Entities.Common;
using AuditSystem.Domain.Entities.Users;

namespace AuditSystem.Domain.Entities.Auth;

public class RefreshToken : Entity<Guid>
{
    public required string Token { get; set; }

    public Guid UserId { get; set; }
    public virtual ApplicationUser User { get; set; } = null!;

    public required DateTime ExpiresAt { get; set; }
    public required bool IsRevoked { get; set; }
}