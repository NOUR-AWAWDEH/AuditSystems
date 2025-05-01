using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.Organization.SubDepartment.Create;

public sealed record class CreateSubDepartmentCommand : ICommand<Result<CreateSubDepartmentCommandResponse>>
{
    public required string Name { get; set; } = string.Empty;
    public required Guid DepartmentId { get; set; }
}
