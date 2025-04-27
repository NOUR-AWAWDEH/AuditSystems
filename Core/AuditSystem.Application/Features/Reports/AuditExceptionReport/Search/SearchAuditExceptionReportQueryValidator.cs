using AuditSystem.Application.Features.Common.Pagination;
using AuditSystem.Contract.Models.Reports;

namespace AuditSystem.Application.Features.Reports.AuditExceptionReport.Search;

public class SearchAuditExceptionReportQueryValidator
    : PaginationRequestValidator<AuditExceptionReportModel, SearchAuditExceptionReportQuery>;