using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.Risks.RiskAssessment.Create;

public sealed record class CreateRiskAssessmentCommand : ICommand<Result<CreateRiskAssessmentCommandResponse>>
{
    public string BusinessObjective { get; set; } = string.Empty;
    public string NatureThrough { get; set; } = string.Empty;
    public string PerformRiskAssessment { get; set; } = string.Empty;
}
