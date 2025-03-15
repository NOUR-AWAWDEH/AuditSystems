using AuditSystem.Contract.Models.Common;

namespace AuditSystem.Contract.Models.Compliance;

public sealed class ComplianceAuditLinkModel : BaseModel<Guid>
{
    public required Guid ComplianceId { get; set; }
    public required Guid AuditUniverseId { get; set; }
    public required string IdentifiedThrough { get; set; } = string.Empty;
    public required Guid InitiatedById { get; set; }
}
