using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.Processes.AuditProcess.Delete;

public sealed record DeleteAuditProcessCommand(
    Guid Id
) : UserAwareCommand<Result>;