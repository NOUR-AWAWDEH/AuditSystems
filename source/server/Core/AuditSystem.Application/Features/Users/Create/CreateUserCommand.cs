using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.Users.Create;

public sealed record CreateUserCommand(
    string UserName,
    string Email,
    string Password,
    string FirstName,
    string LastName,
    string RoleName,
    Guid CompanyId,
    Guid? JobTitleId,
    Guid? SubDepartmentId,
    string Scheme = "https"
) : UserAwareCommand<Result<CreateUserCommandResponse>>;
