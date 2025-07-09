using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.Organization.Department.Create;

public sealed record CreateDepartmentCommand(
    string Name,
    Guid SubLocationId
) : UserAwareCommand<Result<CreateDepartmentCommandResponse>>;