using AuditSystem.Domain.Entities.Audit;
using AuditSystem.Domain.Entities.Common;
using AuditSystem.Domain.Entities.Risks;

namespace AuditSystem.Domain.Entities.Observations;

public class Observation : Entity<Guid>
{
    public required string Description { get; set; } = string.Empty;
    public string Recommendation { get; set; } = string.Empty;
    public required string ActionPlan { get; set; } = string.Empty;
    public required DateTime ActionDate { get; set; }
    public required Guid AuditProgramId { get; set; } = Guid.Empty;

    // Navigation property 
    public virtual AuditProgram? AuditProgram { get; set; } = null!;
}