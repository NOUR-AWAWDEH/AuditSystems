using AuditSystem.Domain.Entities.Common;
using AuditSystem.Domain.Entities.Observations;
using AuditSystem.Domain.Entities.Risks;

namespace AuditSystem.Domain.Entities.Audit;

public class AuditProgram : Entity<Guid>
{
    public required string Description { get; set; } = string.Empty;
    public required Guid ExitControlId { get; set; } = Guid.Empty;
    public Guid? ControlDesignId { get; set; } = Guid.Empty;

    // Navigation properties
    public virtual ControlDesign? ControlDesign { get; set; } 
    public virtual ExitControl ExitControl { get; set; } = null!;
    public virtual ICollection<Observation> Observations { get; set; } = new HashSet<Observation>();
}