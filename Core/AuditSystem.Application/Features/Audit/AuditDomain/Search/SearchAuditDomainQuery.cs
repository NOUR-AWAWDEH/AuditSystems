using AuditSystem.Application.Features.Common.Pagination;
using AuditSystem.Contract.Models.Audit;

namespace AuditSystem.Application.Features.Audit.AuditDomain.Search;

public sealed record class SearchAuditDomainQuery : PaginationRequest<AuditDomainModel>
{
    public string? SearchTearm { get; set; } = string.Empty;
}