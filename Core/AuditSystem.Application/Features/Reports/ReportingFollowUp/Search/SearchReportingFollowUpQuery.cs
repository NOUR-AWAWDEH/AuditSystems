using AuditSystem.Application.Features.Common.Pagination;
using AuditSystem.Contract.Models.Reports;

namespace AuditSystem.Application.Features.Reports.ReportingFollowUp.Search;

public sealed record class SearchReportingFollowUpQuery : PaginationRequest<ReportingFollowUpModel>
{
    public string? SearchTerm { get; set; } = string.Empty;
}