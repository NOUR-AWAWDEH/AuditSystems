using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.Risks.RiskFactor.Create;

public sealed record class CreateRiskFactorCommand : ICommand<Result<CreateRiskFactorCommandResponse>>
{
    public required Guid AuditorSettingsId { get; set; }
    public string Factor { get; set; } = string.Empty;
}
