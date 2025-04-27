using AuditSystem.Application.Features.Common.Pagination;
using AuditSystem.Contract.Models.Reports;

namespace AuditSystem.Application.Features.Reports.JobTimeAllocationReport.Search;

public sealed record class SearchJobTimeAllocationReportQuery : PaginationRequest<JobTimeAllocationReportModel>
{
    public string? SearchTerm { get; set; } = string.Empty;
}