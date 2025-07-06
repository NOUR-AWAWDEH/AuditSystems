using AuditSystem.Application.Features.Common.Pagination;
using AuditSystem.Contract.Models.Organization;

namespace AuditSystem.Application.Features.Organization.SubDepartment.Search;

public sealed record class SearchSubDepartmentQuery : PaginationRequest<SubDepartmentModel>
{
    public string? SearchTerm { get; set; } = string.Empty;
}