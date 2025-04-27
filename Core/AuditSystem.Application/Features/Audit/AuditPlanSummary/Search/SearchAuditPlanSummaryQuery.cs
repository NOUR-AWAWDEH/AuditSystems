using AuditSystem.Application.Features.Common.Pagination;
using AuditSystem.Contract.Models.Audit;

namespace AuditSystem.Application.Features.Audit.AuditPlanSummary.Search;

public sealed record class SearchAuditPlanSummaryQuery : PaginationRequest<AuditPlanSummaryModel>
{
    public string? SearchTearm { get; set; } = string.Empty;
}