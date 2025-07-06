using AuditSystem.Application.Features.Common.Pagination;
using AuditSystem.Contract.Models.Risks;

namespace AuditSystem.Application.Features.Risks.RiskFactor.Search;

public class SearchRiskFactorQueryValidator 
    : PaginationRequestValidator<RiskFactorModel, SearchRiskFactorQuery>;