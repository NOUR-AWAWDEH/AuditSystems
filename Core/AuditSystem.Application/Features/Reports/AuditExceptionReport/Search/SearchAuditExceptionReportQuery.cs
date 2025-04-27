using AuditSystem.Application.Features.Common.Pagination;
using AuditSystem.Contract.Models.Reports;

namespace AuditSystem.Application.Features.Reports.AuditExceptionReport.Search;

public sealed record class SearchAuditExceptionReportQuery : PaginationRequest<AuditExceptionReportModel>
{
    public string? SearchTerm { get; set; } = string.Empty;
}