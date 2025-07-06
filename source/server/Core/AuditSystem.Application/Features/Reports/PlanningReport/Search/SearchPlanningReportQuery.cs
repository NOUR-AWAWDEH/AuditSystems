using AuditSystem.Application.Features.Common.Pagination;
using AuditSystem.Contract.Models.Reports;

namespace AuditSystem.Application.Features.Reports.PlanningReport.Search;

public sealed record class SearchPlanningReportQuery : PaginationRequest<PlanningReportModel>
{
    public string? SearchTerm { get; set; } = string.Empty;
}