using AuditSystem.Domain.Entities.Common;

namespace AuditSystem.Domain.Entities.Users;

public class UserManagement : Entity<Guid>
{
    public Guid AuditorSettingsID { get; set; }
    public Guid UserID { get; set; }
    public string Designation { get; set; } = string.Empty;
    
    public virtual AuditorSettings AuditorSettings { get; set; } = null!;
    public virtual User User { get; set; } = null!;
}