using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.Organization.SubLocation.Create;

public sealed record CreateSubLocationCommand(
    string Name,
    string? Address,
    Guid LocationId
) : UserAwareCommand<Result<CreateSubLocationCommandResponse>>;