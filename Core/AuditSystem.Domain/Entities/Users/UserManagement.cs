using AuditSystem.Domain.Entities.Common;

namespace AuditSystem.Domain.Entities.Users;

public class UserManagement : Entity<Guid>
{
    public required Guid AuditorSettingsID { get; set; }
    public required Guid UserID { get; set; }

    public virtual AuditorSettings AuditorSettings { get; set; } = null!;
    public virtual User User { get; set; } = null!;
}