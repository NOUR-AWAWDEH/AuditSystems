using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.Organization.Location.Delete;

public sealed record DeleteLocationCommand(
    Guid Id
) : UserAwareCommand<Result>;