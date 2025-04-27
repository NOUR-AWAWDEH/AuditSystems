using AuditSystem.Application.Features.Common.Pagination;
using AuditSystem.Contract.Models.RiskControls;

namespace AuditSystem.Application.Features.RiskControls.RiskControls.Search;

public class SearchRiskControlsQueryValidator : PaginationRequestValidator<RiskControlsModel, SearchRiskControlsQuery>;