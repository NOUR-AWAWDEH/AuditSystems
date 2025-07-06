using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.Risks.Risk.Create;

public sealed record class CreateRiskCommand : ICommand<Result<CreateRiskCommandResponse>>
{
    public required Guid ObjectiveId { get; set; }
    public required string RiskName { get; set; } = string.Empty;
    public required Guid RatingId { get; set; }
    public string Description { get; set; } = string.Empty;
}