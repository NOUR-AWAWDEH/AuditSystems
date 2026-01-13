using AuditSystem.Application.Features.Common.Pagination;
using AuditSystem.Contract.Interfaces;
using AuditSystem.Contract.Models.Organization;

namespace AuditSystem.Application.Features.Organization.Company.Search;

public sealed record class SearchCompanyQuery : PaginationRequest<CompanyModel>, IUserAwareRequest
{
    public string? SearchTerm { get; set; } = string.Empty;
    public Guid UserId { get; set; }
}