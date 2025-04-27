using AuditSystem.Application.Features.Common.Pagination;
using AuditSystem.Contract.Models.Audit;

namespace AuditSystem.Application.Features.Audit.AuditEngagement.Search;

public sealed record class SearchAuditEngagementQuery : PaginationRequest<AuditEngagementModel>
{
    public string? SearchTerm { get; set; } = string.Empty;
}