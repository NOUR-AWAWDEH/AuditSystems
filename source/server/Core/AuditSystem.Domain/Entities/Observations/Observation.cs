using AuditSystem.Domain.Entities.Audit;
using AuditSystem.Domain.Entities.Common;
using AuditSystem.Domain.Entities.Recommendations;

namespace AuditSystem.Domain.Entities.Observations;

public class Observation : AuditableEntity<Guid>
{
    public required string Highlight { get; set; }
    public required string Background { get; set; }
    public required bool IsActive { get; set; }
    public string? Note { get; set; } = string.Empty;
    public required Guid AuditProgramId { get; set; }
    public required Guid RateId { get; set; }

    // Navigation property 
    public virtual AuditProgram AuditProgram { get; set; } = null!;
    public virtual Rate Rate { get; set; } = null!;
    public virtual ICollection<Recommendation> Recommendations { get; set; } = new List<Recommendation>();
}