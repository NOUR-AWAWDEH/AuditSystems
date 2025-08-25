using AuditSystem.Domain.Entities.Common;

namespace AuditSystem.Domain.Entities.Risks;
public class ControlDesign : Entity<Guid>
{
    public required string Description { get; set; } = string.Empty;
    public required Guid ExitControlId { get; set; } = Guid.Empty;
    public required bool Inadequate { get; set; } = true;

    // Navigation properties
    public ExitControl ExitControl { get; set; } = null!;
}