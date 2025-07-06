using AuditSystem.Contract.Models.Common;

namespace AuditSystem.Contract.Models.Compliance;

public sealed class ComplianceChecklistModel : BaseModel<Guid>
{
    public required string Area { get; set; } = string.Empty;
    public required string Subject { get; set; } = string.Empty;
    public required string Particulars { get; set; } = string.Empty;
}
