using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.Organization.Location.Create;

public sealed record CreateLocationCommand(
    string Name,
    string? Address,
    Guid CompanyId
) : UserAwareCommand<Result<CreateLocationCommandResponse>>;