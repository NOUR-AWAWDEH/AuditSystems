using AuditSystem.Application.Features.Common.Pagination;
using AuditSystem.Contract.Models.Organization;

namespace AuditSystem.Application.Features.Organization.Department.Search;

public sealed record class SearchDepartmentQuery : PaginationRequest<DepartmentModel>
{
    public string? SearchTerm { get; set; } = string.Empty;
}