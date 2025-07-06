using AuditSystem.Application.Features.Common.Pagination;
using AuditSystem.Contract.Models.Audit;

namespace AuditSystem.Application.Features.Audit.AuditPlanSummary.Search;

public class SearchAuditPlanSummaryQueryValidator 
    : PaginationRequestValidator<AuditPlanSummaryModel, SearchAuditPlanSummaryQuery>;