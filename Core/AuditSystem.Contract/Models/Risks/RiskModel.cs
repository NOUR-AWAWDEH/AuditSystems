using AuditSystem.Contract.Models.Common;

namespace AuditSystem.Contract.Models.Risks;

public sealed class RiskModel : BaseModel<Guid>
{
    public required string RiskName { get; init; }
    public required string Description { get; init; }
    public required string Rating { get; init; }
    public required Guid ObjectiveId { get; init; }
}