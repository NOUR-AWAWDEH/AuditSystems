using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.Organization.SubLocation.Delete;

public sealed record DeleteSubLocationCommand(
    Guid Id
) : UserAwareCommand<Result>;