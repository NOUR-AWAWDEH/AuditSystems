using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.Risks.ExitControl.Delete;

public sealed record DeleteExitControlCommand(
    Guid Id
) : UserAwareCommand<Result>;