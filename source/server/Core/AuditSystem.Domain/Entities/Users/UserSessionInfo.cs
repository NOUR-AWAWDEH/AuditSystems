using AuditSystem.Domain.Entities.Common;

namespace AuditSystem.Domain.Entities.Users;

public class UserSessionInfo : Entity<Guid>
{
    public required Guid AppUserId { get; set; }
    public virtual AppUser AppUser { get; set; } = null!;

    public DateTime? CurrentSessionStart { get; set; }
    public TimeSpan TotalSessionTime { get; set; } = TimeSpan.Zero;
    public DateTime? LastLogin { get; set; }
    public bool IsActive { get; set; } = false;
}
