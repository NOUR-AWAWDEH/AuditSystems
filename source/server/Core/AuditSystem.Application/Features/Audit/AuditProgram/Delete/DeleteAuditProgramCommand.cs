using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.Audit.AuditProgram.Delete;

public sealed record DeleteAuditProgramCommand(
    Guid Id
) : UserAwareCommand<Result>;