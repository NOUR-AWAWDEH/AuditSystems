using AuditSystem.Application.Features.Common.Pagination;
using AuditSystem.Contract.Models.Reports;

namespace AuditSystem.Application.Features.Reports.AuditPlanSummaryReport.Search;

public sealed record class SearchAuditPlanSummaryReportQuery : PaginationRequest<AuditPlanSummaryReportModel>
{
    public string? SearchTerm { get; set; } = string.Empty;
}
