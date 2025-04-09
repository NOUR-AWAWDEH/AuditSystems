using AuditSystem.Domain.Entities.Common;

namespace AuditSystem.Domain.Entities.Users;

public class UserManagement : Entity<Guid>
{
    public required Guid UserID { get; set; }
    public required string Designation { get; set; } = string.Empty;
    public virtual User User { get; set; } = null!;
}