using AuditSystem.Domain.Entities.Common;

namespace AuditSystem.Domain.Entities.Users;

public class UserDesignation : Entity<Guid>
{
    public required string Designation { get; set; } = string.Empty;
    public required string Level { get; set; }
    public required bool IsActive { get; set; } = true;
    public required Guid UserId { get; set; }
    public string Description { get; set; } = string.Empty;

    public virtual User User { get; set; } = null!;
}