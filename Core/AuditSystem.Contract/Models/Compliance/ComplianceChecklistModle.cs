using AuditSystem.Contract.Models.Common;

namespace AuditSystem.Contract.Models.Compliance;

public sealed class ComplianceChecklistModle : BaseModel<Guid>
{
    public int SerialNumber { get; set; }
    public string Area { get; set; } = string.Empty;
    public string Subject { get; set; } = string.Empty;
    public string Particulars { get; set; } = string.Empty;
}
