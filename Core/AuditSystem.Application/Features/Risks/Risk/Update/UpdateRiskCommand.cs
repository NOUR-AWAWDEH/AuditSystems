using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.Risks.Risk.Update;

public sealed record class UpdateRiskCommand : ICommand<Result>
{
    public required Guid Id { get; init; } = Guid.Empty;
    public required Guid ObjectiveId { get; set; }
    public required string RiskName { get; set; } = string.Empty;
    public required Guid RatingId { get; set; }
    public string Description { get; set; } = string.Empty;
}