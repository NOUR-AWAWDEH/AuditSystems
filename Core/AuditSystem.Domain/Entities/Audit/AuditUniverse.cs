using AuditSystem.Domain.Entities.Audit;
using AuditSystem.Domain.Entities.Common;

namespace AuditSystem.Domain.Entities;

public class AuditUniverse : Entity<Guid>
{
    public required string BusinessObjective { get; set; } = string.Empty;
    public string IndustryUpdate { get; set; } = string.Empty;
    public string CompanyUpdate { get; set; } = string.Empty;
    public required Guid DomainId { get; set; }
    public required bool IsFinancialQuantifiable { get; set; } = false;

    public required bool IsSpecialProject { get; set; } = false;
    public Guid SpecialProjectId { get; set; }

    public virtual AuditDomain Domain { get; set; } = null!;
    public virtual SpecialProject? SpecialProject { get; set; }
}
