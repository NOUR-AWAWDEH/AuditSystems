using AuditSystem.Application.Features.Common.Pagination;
using AuditSystem.Contract.Models.RiskControls;

namespace AuditSystem.Application.Features.RiskControls.RiskControlMatrix.Search;

public class SearchRiskControlMatrixQueryValidator : PaginationRequestValidator<RiskControlMatrixModel, SearchRiskControlMatrixQuery>;