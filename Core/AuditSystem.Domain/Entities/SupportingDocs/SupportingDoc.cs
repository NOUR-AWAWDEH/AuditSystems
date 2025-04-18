using AuditSystem.Domain.Entities.Common;

namespace AuditSystem.Domain.Entities.SupportingDocs;

public class SupportingDoc : Entity<Guid>
{
    public required string FileName { get; set; } = string.Empty;
    public required int FileSize { get; set; }
    public required string URL { get; set; } = string.Empty;
}