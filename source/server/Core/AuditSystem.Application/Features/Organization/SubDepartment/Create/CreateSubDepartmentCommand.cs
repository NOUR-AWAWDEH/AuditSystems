using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.Organization.SubDepartment.Create;

public sealed record class CreateSubDepartmentCommand(
    string Name,
    Guid DepartmentId
) : UserAwareCommand<Result<CreateSubDepartmentCommandResponse>>;