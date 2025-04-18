using AuditSystem.Domain.Entities.Common;

namespace AuditSystem.Domain.Entities.Compliance;

public class ComplianceAuditLink : Entity<Guid>
{
    public required Guid ComplianceId { get; set; }
    public required Guid AuditUniverseId { get; set; }
    public required Guid InitiatedById { get; set; }

    public virtual ComplianceChecklist ComplianceChecklist { get; set; } = null!;
    public virtual AuditUniverse AuditUniverse { get; set; } = null!;
    public virtual User Initiator { get; set; } = null!;
}