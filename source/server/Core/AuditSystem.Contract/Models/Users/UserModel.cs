using AuditSystem.Contract.Models.Organization;

namespace AuditSystem.Contract.Models.Users;

public sealed record UserModel(
    Guid Id,
    string FirstName,
    string LastName,
    string Email,
    string Role,
    SubDepartmentModel? SubDepartment,
    JobTitleModel? JobTitle);
