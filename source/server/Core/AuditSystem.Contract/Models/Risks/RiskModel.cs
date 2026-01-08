using AuditSystem.Contract.Models.Common;

namespace AuditSystem.Contract.Models.Risks;

public sealed class RiskModel : BaseModel<Guid>
{
    public required string Factor { get; init; }
    public required Guid ObservationId { get; init; }
    public string Impact { get; init; } = string.Empty;
}