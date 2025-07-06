using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.RiskControls.RiskControls.Update;

public sealed record class UpdateRiskControlsCommand : ICommand<Result>
{
    public required Guid Id { get; init; } = Guid.Empty;
    public required Guid RiskId { get; set; }
    public required Guid RatingId { get; set; }
    public string Description { get; set; } = string.Empty;
}