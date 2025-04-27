using AuditSystem.Application.Features.Common.Pagination;
using AuditSystem.Contract.Models.Reports;

namespace AuditSystem.Application.Features.Reports.JobTimeAllocationReport.Search;

public class SearchJobTimeAllocationReportQueryValidator 
    : PaginationRequestValidator<JobTimeAllocationReportModel, SearchJobTimeAllocationReportQuery>;