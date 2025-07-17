using AuditSystem.Application.Features.Common.Pagination;
using AuditSystem.Contract.Interfaces;
using AuditSystem.Contract.Models.Organization;

namespace AuditSystem.Application.Features.Organization.Department.Search;

public sealed record SearchDepartmentQuery(
    string? SearchTerm,
    Guid SubLocationId
) : PaginationRequest<DepartmentModel>, IUserAwareRequest
{
    public Guid UserId { get; set; }
}