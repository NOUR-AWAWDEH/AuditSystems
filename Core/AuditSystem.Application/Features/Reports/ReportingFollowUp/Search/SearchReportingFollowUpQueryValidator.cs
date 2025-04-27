using AuditSystem.Application.Features.Common.Pagination;
using AuditSystem.Contract.Models.Reports;

namespace AuditSystem.Application.Features.Reports.ReportingFollowUp.Search;

public class SearchReportingFollowUpQueryValidator : PaginationRequestValidator<ReportingFollowUpModel, SearchReportingFollowUpQuery>;