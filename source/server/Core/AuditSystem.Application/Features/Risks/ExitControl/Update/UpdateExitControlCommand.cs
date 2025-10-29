using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.Risks.ExitControl.Update;

public sealed record class UpdateExitControlCommand(
    Guid Id,
    string Description,
    Guid ObjectiveId,
    bool Inadequate
) : UserAwareCommand<Result>;
