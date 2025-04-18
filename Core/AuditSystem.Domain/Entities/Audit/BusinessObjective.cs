using AuditSystem.Domain.Entities.Common;

namespace AuditSystem.Domain.Entities.Audit;

public class BusinessObjective : Entity<Guid>
{
    public required string Impact { get; set; } = string.Empty;
    public required int Amount { get; set; }
}