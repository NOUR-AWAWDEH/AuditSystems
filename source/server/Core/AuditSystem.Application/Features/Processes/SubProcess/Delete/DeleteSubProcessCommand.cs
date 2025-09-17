using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.Processes.SubProcess.Delete;

public sealed record DeleteSubProcessCommand(
    Guid Id
) : UserAwareCommand<Result>;
