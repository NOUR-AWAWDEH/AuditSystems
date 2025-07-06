using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.Organization.SubDepartment.Delete;

public sealed class DeleteSubDepartmentCommand : ICommand<Result>
{
    public required Guid Id { get; set; } = Guid.Empty;
}