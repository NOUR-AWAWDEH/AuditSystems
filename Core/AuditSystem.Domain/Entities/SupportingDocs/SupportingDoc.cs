using AuditSystem.Domain.Entities.Users;
using AuditSystem.Domain.Entities.Common;

namespace AuditSystem.Domain.Entities.SupportingDocs;

public class SupportingDoc : Entity<Guid>
{
    public required Guid AuditorSettingsId { get; set; }
    public string FileName { get; set; } = string.Empty;
    public int FileSize {get; set;}
    public string URL {get; set;} = string.Empty;

    public virtual AuditorSettings AuditorSettings { get; set; } = null!;
}