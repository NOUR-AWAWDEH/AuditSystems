using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.Organization.Location.Update;

public sealed record UpdateLocationCommand(
    Guid Id,
    string Name,
    string? Address,
    Guid CompanyId
) : UserAwareCommand<Result>;