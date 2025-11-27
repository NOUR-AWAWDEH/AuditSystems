using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.Audit.AuditProgram.Create;

public sealed record CreateAuditProgramCommand(
    string Description,
    Guid ExitControlId,
    Guid ActionOwnerId,
    Guid? ControlDesignId
) : UserAwareCommand<Result<CreateAuditProgramCommandResponse>>;