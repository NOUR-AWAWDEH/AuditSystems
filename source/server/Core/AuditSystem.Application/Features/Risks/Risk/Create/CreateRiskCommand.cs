using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.Risks.Risk.Create;

public sealed record class CreateRiskCommand(
    string RiskName,
    string Description,
    Guid ObservationId
) : UserAwareCommand<Result<CreateRiskCommandResponse>>;
