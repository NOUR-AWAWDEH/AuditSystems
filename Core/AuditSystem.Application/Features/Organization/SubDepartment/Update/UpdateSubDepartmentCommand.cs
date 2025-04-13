using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.Organisation.SubDepartment.Update;

public sealed record class UpdateSubDepartmentCommand : ICommand<Result>
{
    public required Guid Id { get; init; } = Guid.Empty;
    public required string Name { get; set; } = string.Empty;
    public required Guid DepartmentId { get; set; }
}
