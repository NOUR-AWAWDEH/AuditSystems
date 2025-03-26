using AuditSystem.Domain.Entities.Common;
using AuditSystem.Domain.Entities.Users;

namespace AuditSystem.Domain.Entities.Organisation;

public class Location : Entity<Guid>
{
    public required Guid AuditorSettingsId { get; set; }
    public string Name { get; set; } = string.Empty;
    
    public virtual AuditorSettings AuditorSettings { get; set; } = null!;
}