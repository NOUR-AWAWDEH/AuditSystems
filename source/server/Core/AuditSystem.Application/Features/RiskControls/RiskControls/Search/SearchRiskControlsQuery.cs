using AuditSystem.Application.Features.Common.Pagination;
using AuditSystem.Contract.Models.RiskControls;

namespace AuditSystem.Application.Features.RiskControls.RiskControls.Search;

public sealed record class SearchRiskControlsQuery : PaginationRequest<RiskControlsModel>
{
    public string? SearchTerm { get; set; } = string.Empty;
}