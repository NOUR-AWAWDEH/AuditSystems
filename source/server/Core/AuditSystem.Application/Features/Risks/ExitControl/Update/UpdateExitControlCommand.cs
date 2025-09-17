using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.Risks.ExitControl.Update;

public sealed record class UpdateExitControlCommand(
    Guid Id,
    string Description,
    Guid RiskId,
    bool Inadequate
) : UserAwareCommand<Result>;
