using AuditSystem.Application.Features.Common.Pagination;
using AuditSystem.Contract.Models.Reports;

namespace AuditSystem.Application.Features.Reports.InternalAuditConsolidationReport.Search;

public sealed record class SearchInternalAuditConsolidationReportQuery : PaginationRequest<InternalAuditConsolidationReportModel>
{
    public string? SearchTerm { get; set; } = string.Empty;
}