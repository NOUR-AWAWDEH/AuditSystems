using AuditSystem.Application.Features.Common.Pagination;
using AuditSystem.Contract.Interfaces;
using AuditSystem.Contract.Models.Organization;

namespace AuditSystem.Application.Features.Organization.Department.Search;

public sealed record SearchDepartmentQuery(
    string? SearchTerm,
    Guid LocationId
) : PaginationRequest<DepartmentModel>, IUserAwareRequest
{
    public Guid UserId { get; set; }
}