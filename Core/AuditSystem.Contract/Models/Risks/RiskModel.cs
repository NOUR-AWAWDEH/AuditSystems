using AuditSystem.Contract.Models.Common;

namespace AuditSystem.Contract.Models.Risks;

public sealed class RiskModel : BaseModel<Guid>
{
    public required string RiskName { get; init; }
    public required Guid RatingId { get; init; }
    public required Guid ObjectiveId { get; init; }
    public string Description { get; init; } = string.Empty;
}