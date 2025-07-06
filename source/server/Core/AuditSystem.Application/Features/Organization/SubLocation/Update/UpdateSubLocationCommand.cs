using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.Organization.SubLocation.Update;

public sealed record UpdateSubLocationCommand(
    Guid Id,
    string Name,
    string? Address,
    Guid LocationId
) : UserAwareCommand<Result>;