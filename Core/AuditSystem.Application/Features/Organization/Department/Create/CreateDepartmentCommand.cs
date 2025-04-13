using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.Organisation.Department.Create;

public sealed record class CreateDepartmentCommand : ICommand<Result<CreateDepartmentCommandResponse>>
{
    public required string Name { get; set; } = string.Empty;
}
