using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.Risks.RiskFactor.Update;

public sealed record class UpdateRiskFactorCommand : ICommand<Result>
{
    public required Guid Id { get; init; } = Guid.Empty;
    public required string Factor { get; set; } = string.Empty;
}
