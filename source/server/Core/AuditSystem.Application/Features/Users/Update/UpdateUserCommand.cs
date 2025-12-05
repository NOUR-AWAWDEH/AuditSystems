using Ardalis.Result;
using AuditSystem.Application.Base;
using AuditSystem.Contract.Models.Organization;

namespace AuditSystem.Application.Features.Users.Update;

public sealed record UpdateUserCommand(
    Guid Id,
    string UserName,
    string FirstName,
    string LastName,
    string Email,
    string Role,
    SubDepartmentModel? SubDepartment,
    JobTitleModel? JobTitle
) : UserAwareCommand<Result>;
