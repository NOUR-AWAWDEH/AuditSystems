using AuditSystem.Contract.Models.Common;

namespace AuditSystem.Contract.Models.Audit;

public sealed class AuditPlanSummaryModel : BaseModel<Guid>
{
    public required string Component { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string ExampleDetails { get; set; } = string.Empty;
}
