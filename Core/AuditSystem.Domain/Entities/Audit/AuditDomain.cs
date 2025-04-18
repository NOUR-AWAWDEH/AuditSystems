using AuditSystem.Domain.Entities.Common;

namespace AuditSystem.Domain.Entities.Audit;

public class AuditDomain : Entity<Guid>
{
    public required string Name { get; set; } = string.Empty;
}