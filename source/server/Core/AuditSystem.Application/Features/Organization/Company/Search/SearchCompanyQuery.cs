using AuditSystem.Application.Features.Common.Pagination;
using AuditSystem.Contract.Models.Organization;

namespace AuditSystem.Application.Features.Organization.Company.Search;

public sealed record class SearchCompanyQuery : PaginationRequest<CompanyModel>
{
    public string? SearchTerm { get; set; } = string.Empty;
}