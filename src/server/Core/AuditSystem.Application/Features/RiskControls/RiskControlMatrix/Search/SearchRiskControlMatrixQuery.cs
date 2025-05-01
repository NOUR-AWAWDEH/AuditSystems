using AuditSystem.Application.Features.Common.Pagination;
using AuditSystem.Contract.Models.RiskControls;

namespace AuditSystem.Application.Features.RiskControls.RiskControlMatrix.Search;

public sealed record class SearchRiskControlMatrixQuery : PaginationRequest<RiskControlMatrixModel>
{
    public string? SearchTerm { get; set; } = string.Empty;
}