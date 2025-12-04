using AuditSystem.Contract.Models.Common;

namespace AuditSystem.Contract.Models.SupportingDocs;

public sealed class SupportingDocModel : BaseModel<Guid>
{
    public required string FileName { get; set; }
    public required long FileSize { get; set; }
    public Guid DriveFileId { get; set; } = Guid.Empty;
    public Guid CreatedByUserId { get; set; }

    public string FileType => Path.GetExtension(FileName)?.TrimStart('.').ToUpperInvariant() ?? "Unknown";
}
