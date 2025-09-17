using AuditSystem.Contract.Models.Common;

namespace AuditSystem.Contract.Models.SupportingDocs;

public sealed class SupportingDocModel : BaseModel<Guid>
{
    public required Guid UserId { get; set; }
    public required string FileName { get; set; }
    public required long FileSize { get; set; }
    public Guid DriveFileId { get; set; } = Guid.Empty;
    
    public string FileType => Path.GetExtension(FileName)?.TrimStart('.').ToUpperInvariant() ?? "Unknown";
}
