using AuditSystem.Domain.Entities.Common;

namespace AuditSystem.Domain.Entities.Audit;

public class SpecialProject : Entity<Guid>
{
    public Guid AuditUniverseId { get; set; }
    public string ProjectName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime StartDate { get; set; } = DateTime.UtcNow;
    public DateTime? EndDate { get; set; }

    public virtual AuditUniverse AuditUniverse { get; set; } = null!;
}
