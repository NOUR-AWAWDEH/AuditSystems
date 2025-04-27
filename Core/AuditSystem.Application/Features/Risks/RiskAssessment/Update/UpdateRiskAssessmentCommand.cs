using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.Risks.RiskAssessment.Update;

public sealed record class UpdateRiskAssessmentCommand : ICommand<Result>
{
    public required Guid Id { get; init; } = Guid.Empty;
    public required string BusinessObjective { get; set; } = string.Empty;
    public required string NatureThrough { get; set; } = string.Empty;
    public required string PerformRiskAssessment { get; set; } = string.Empty;
}
