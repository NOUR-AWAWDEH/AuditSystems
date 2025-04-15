using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.Risks.RiskAssessment.Delete;

public sealed class DeleteRiskAssessmentCommand : ICommand<Result>
{
    public required Guid Id { get; set; } = Guid.Empty;
}