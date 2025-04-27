using AuditSystem.Application.Features.Common.Pagination;
using AuditSystem.Contract.Models.Risks;

namespace AuditSystem.Application.Features.Risks.Risk.Search;

public class SearchRiskQueryValidator : PaginationRequestValidator<RiskModel, SearchRiskQuery>;