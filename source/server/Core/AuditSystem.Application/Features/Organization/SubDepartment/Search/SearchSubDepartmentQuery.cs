using AuditSystem.Application.Features.Common.Pagination;
using AuditSystem.Contract.Interfaces;
using AuditSystem.Contract.Models.Organization;

namespace AuditSystem.Application.Features.Organization.SubDepartment.Search;

public sealed record class SearchSubDepartmentQuery(
    string? SearchTerm,
    Guid DepartmentId
) : PaginationRequest<SubDepartmentModel>, IUserAwareRequest
{
    public Guid UserId { get; set; }
}