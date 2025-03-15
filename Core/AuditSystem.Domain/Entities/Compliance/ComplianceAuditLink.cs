using AuditSystem.Domain.Entities.Audit;
using AuditSystem.Domain.Entities.Users;
using AuditSystem.Domain.Entities.Common;

namespace AuditSystem.Domain.Entities.Compliance;

public class ComplianceAuditLink : Entity<Guid> // many to many 
{
    public Guid ComplianceId { get; set; }
    public Guid AuditUniverseId { get; set; }
    public string IdentifiedThrough { get; set; } = string.Empty;
    public Guid InitiatedById { get; set; }

    public virtual ComplianceChecklist ComplianceChecklist { get; set; } = null!;
    public virtual AuditUniverse AuditUniverse { get; set; } = null!;
    public virtual User Initiator { get; set; } = null!;
}