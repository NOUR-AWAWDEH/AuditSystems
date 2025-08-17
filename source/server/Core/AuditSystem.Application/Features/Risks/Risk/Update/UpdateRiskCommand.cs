using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.Risks.Risk.Update;

public sealed record class UpdateRiskCommand(
    Guid Id,
    Guid ObjectiveId,
    string RiskName,
    Guid RateId,
    string Description
) : UserAwareCommand<Result>;
