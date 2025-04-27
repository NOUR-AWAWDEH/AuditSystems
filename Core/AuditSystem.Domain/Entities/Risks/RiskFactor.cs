using AuditSystem.Domain.Entities.Common;

namespace AuditSystem.Domain.Entities.Risks;

public class RiskFactor : Entity<Guid>
{
    public required string Factor { get; set; } = string.Empty;
}