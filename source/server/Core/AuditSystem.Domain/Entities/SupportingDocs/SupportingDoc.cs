using AuditSystem.Domain.Entities.Common;
using AuditSystem.Domain.Entities.Users;

namespace AuditSystem.Domain.Entities.SupportingDocs;

public class SupportingDoc : Entity<Guid>
{

    public Guid UserId { get; set; } = Guid.Empty;
    public required string FileName { get; set; }
    public Guid DriveFileId { get; set; } = Guid.Empty;
    public required long FileSize { get; set; }

    public User User { get; set; } = null!;
}