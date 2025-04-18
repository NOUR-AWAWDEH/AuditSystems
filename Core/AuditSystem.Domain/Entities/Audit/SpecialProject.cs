using AuditSystem.Domain.Entities.Common;

namespace AuditSystem.Domain.Entities.Audit;

public class SpecialProject : Entity<Guid>
{
    public required Guid AuditUniverseId { get; set; }
    public required string ProjectName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public required DateTime StartDate { get; set; } = DateTime.UtcNow;
    public required DateTime? EndDate { get; set; }

    public virtual AuditUniverse AuditUniverse { get; set; } = null!;
}