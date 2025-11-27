using AuditSystem.Domain.Entities.Common;
using AuditSystem.Domain.Entities.Users;

namespace AuditSystem.Domain.Entities.SupportingDocs;

public class SupportingDoc : AuditableEntity<Guid>
{
    public required string FileName { get; set; }
    public Guid DriveFileId { get; set; } = Guid.Empty;
    public required long FileSize { get; set; }
}