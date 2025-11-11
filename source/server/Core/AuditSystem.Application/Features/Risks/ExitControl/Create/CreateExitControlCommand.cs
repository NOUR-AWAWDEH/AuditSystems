using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.Risks.ExitControl.Create;

public sealed record class CreateExitControlCommand(
    string Description,
    Guid ObjectiveId,
    bool Inadequate
) : UserAwareCommand<Result<CreateExitControlCommandResponse>>;
