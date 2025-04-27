using AuditSystem.Application.Features.Common.Pagination;
using AuditSystem.Contract.Models.Reports;

namespace AuditSystem.Application.Features.Reports.PlanningReport.Search;

public class SearchPlanningReportQueryValidator : PaginationRequestValidator<PlanningReportModel, SearchPlanningReportQuery>;