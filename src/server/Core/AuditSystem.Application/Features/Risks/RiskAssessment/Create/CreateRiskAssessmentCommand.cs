using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.Risks.RiskAssessment.Create;

public sealed record class CreateRiskAssessmentCommand : ICommand<Result<CreateRiskAssessmentCommandResponse>>
{
    public required string BusinessObjective { get; set; } = string.Empty;
    public required string NatureThrough { get; set; } = string.Empty;
    public required string PerformRiskAssessment { get; set; } = string.Empty;
}
