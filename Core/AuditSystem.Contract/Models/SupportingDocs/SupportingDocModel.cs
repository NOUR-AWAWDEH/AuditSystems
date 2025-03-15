using AuditSystem.Contract.Models.Common;

namespace AuditSystem.Contract.Models.SupportingDocs;

public sealed class SupportingDocModel : BaseModel<Guid>
{
    public Guid AuditorSettingsId { get; set; }
    public string FileName { get; set; } = string.Empty;
    public int FileSize { get; set; }
    public string URL { get; set; } = string.Empty;
}
