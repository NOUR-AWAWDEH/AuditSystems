using AuditSystem.Contract.Models.Organization;
using AuditSystem.Domain.Entities.Organization;

namespace AuditSystem.Contract.Models.Users;

public sealed record UserModel(
    Guid Id,
    string FirstName,
    string LastName,
    string Email,
    string Role,
    SubDepartment SubDepartment,
    JobTitleModel JobTitle);
