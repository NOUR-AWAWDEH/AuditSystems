using AuditSystem.Domain.Entities.Audit;
using AuditSystem.Domain.Entities.Common;


namespace AuditSystem.Domain.Entities;

public class AuditUniverse : Entity<Guid>
{
    public string BusinessObjective { get; set; } = string.Empty;
    public string IndustryUpdate { get; set; } = string.Empty;
    public string CompanyUpdate { get; set; } = string.Empty;
    public Guid DomainId { get; set; }
    public bool IsFinancialQuantifiable { get; set; }

    public bool IsSpecialProject { get; set; } = false;

    public virtual AuditDomain Domain { get; set; } = null!;

    public virtual SpecialProject? SpecialProject { get; set; }
}
