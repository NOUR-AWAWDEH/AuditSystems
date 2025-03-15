using AuditSystem.Domain.Entities.Common;

namespace AuditSystem.Contract.Models.Audit;

public class AuditDomainModel : Entity<Guid>
{
    public required string Name { get; set; } = string.Empty;
}
