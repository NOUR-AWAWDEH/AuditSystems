using AuditSystem.Application.Features.Common.Pagination;
using AuditSystem.Contract.Models.Reports;

namespace AuditSystem.Application.Features.Reports.AuditPlanSummaryReport.Search;

public class SearchAuditPlanSummaryReportQueryValidator
    : PaginationRequestValidator<AuditPlanSummaryReportModel, SearchAuditPlanSummaryReportQuery>;