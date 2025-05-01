using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.Risks.RiskFactor.Delete;

public sealed class DeleteRiskFactorCommand : ICommand<Result>
{
    public required Guid Id { get; set; } = Guid.Empty;
}