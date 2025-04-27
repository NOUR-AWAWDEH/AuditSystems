using AuditSystem.Application.Features.Common.Pagination;
using AuditSystem.Contract.Models.Risks;

namespace AuditSystem.Application.Features.Risks.RiskAssessment.Search;

public class SearchRiskAssessmentQueryValidator 
    : PaginationRequestValidator<RiskAssessmentModel, SearchRiskAssessmentQuery>;