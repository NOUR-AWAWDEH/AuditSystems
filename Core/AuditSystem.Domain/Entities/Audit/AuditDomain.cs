using AuditSystem.Domain.Entities.Common;

namespace AuditSystem.Domain.Entities.Audit;

public class AuditDomain : Entity<Guid>
{
    public string Name { get; set; } = string.Empty;
}