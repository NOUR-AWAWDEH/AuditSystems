using AuditSystem.Application.Features.Common.Pagination;
using AuditSystem.Contract.Models.RiskControls;

namespace AuditSystem.Application.Features.RiskControls.RiskProgram.Search;

public class SearchRiskProgramQueryValidator : PaginationRequestValidator<RiskProgramModel, SearchRiskProgramQuery>;