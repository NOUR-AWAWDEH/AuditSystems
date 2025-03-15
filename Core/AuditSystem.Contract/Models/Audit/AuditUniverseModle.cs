using AuditSystem.Contract.Models.Common;

namespace AuditSystem.Contract.Models.Audit;

public sealed class AuditUniverseModle : BaseModel<Guid>
{
    public string BusinessObjective { get; set; } = string.Empty;
    public string IndustryUpdate { get; set; } = string.Empty;
    public string CompanyUpdate { get; set; } = string.Empty;
    public required Guid DomainId { get; set; }
    public bool IsFinancialQuantifiable { get; set; }
    public bool IsSpecialProject { get; set; } = false;
}