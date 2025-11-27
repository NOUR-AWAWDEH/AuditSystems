using AuditSystem.Domain.Entities.Audit;
using AuditSystem.Domain.Entities.Common;
using AuditSystem.Domain.Entities.RiskControls;

namespace AuditSystem.Domain.Entities.Risks;

public class ExitControl : AuditableEntity<Guid>
{
    public required Guid ObjectiveId { get; set; }
    public required string Description { get; set; }
    public required bool Inadequate { get; set; }

    // Navigation properties
    public virtual Objective Objective { get; set; } = null!;
    public ICollection<AuditProgram> AuditPrograms { get; set; } = [];
    public ICollection<ControlDesign> ControlDesigns { get; set; } = [];
}