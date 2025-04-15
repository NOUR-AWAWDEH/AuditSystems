using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.Organization.Department.Delete;

public sealed class DeleteDepartmentCommand : ICommand<Result>
{
    public required Guid Id { get; set; } = Guid.Empty;
}
