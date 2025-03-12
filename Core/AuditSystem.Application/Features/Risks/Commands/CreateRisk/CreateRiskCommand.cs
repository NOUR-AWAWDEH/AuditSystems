using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.Risks.Commands.CreateRisk;

public sealed record class CreateRiskCommand : ICommand<Result<CreateRiskCommandResponse>>
{
    public required string RiskName { get; set; } = string.Empty;
    public required string Rating { get; set; } = string.Empty; 
    public required string Description { get; set; } = string.Empty;
    public required Guid ObjectiveId { get; init; }
}