using AuditSystem.Domain.Entities.Audit;
using AuditSystem.Domain.Entities.Common;

namespace AuditSystem.Domain.Entities.Risks;

public class ControlDesign : Entity<Guid>
{
    public required string Description { get; set; }
    public required Guid ExitControlId { get; set; }

    // Navigation properties
    public ExitControl ExitControl { get; set; } = null!;
    public virtual ICollection<AuditProgram> AuditPrograms { get; set; } = [];
}