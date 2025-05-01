using AuditSystem.Contract.Models.Common;

namespace AuditSystem.Contract.Models.SupportingDocs;

public sealed class SupportingDocModel : BaseModel<Guid>
{
    public required string FileName { get; set; } = string.Empty;
    public required int FileSize { get; set; }
    public required string URL { get; set; } = string.Empty;
}
