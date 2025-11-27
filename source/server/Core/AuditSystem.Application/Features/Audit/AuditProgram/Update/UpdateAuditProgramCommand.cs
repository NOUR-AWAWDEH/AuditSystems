using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.Audit.AuditProgram.Update;

public sealed record UpdateAuditProgramCommand(
    Guid Id,
    string Description,
    Guid ExitControlId,
    Guid ActionOwnerId,
    Guid? ControlDesignId
) : UserAwareCommand<Result>;