using AuditSystem.Domain.Entities.Audit;
using AuditSystem.Domain.Entities.Common;

namespace AuditSystem.Domain.Entities.Risks;

public class Risk : Entity<Guid>
{
    public required Guid ObjectiveId { get; set; }
    public required string RiskName { get; set; } = string.Empty;
    public required Guid RateId { get; set; }
    public string Description { get; set; } = string.Empty;

    // Navigation properties
    public virtual Rate Rate { get; set; } = null!;
    public virtual Objective Objective { get; set; } = null!;
    public virtual ICollection<ExitRisk> ExitRisks { get; set; } = new HashSet<ExitRisk>();
}