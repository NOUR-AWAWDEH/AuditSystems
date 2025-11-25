using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.Risks.Risk.Update;

public sealed record class UpdateRiskCommand(
    Guid Id,
    string RiskName,
    string Description,
    Guid ObjectiveId
) : UserAwareCommand<Result>;
