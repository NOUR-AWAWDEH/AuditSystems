using AuditSystem.Application.Features.Common.Pagination;
using AuditSystem.Contract.Models.Risks;

namespace AuditSystem.Application.Features.Risks.Risk.Search;

public sealed record class SearchRiskQuery : PaginationRequest<RiskModel>
{
    public string? SearchTerm { get; set; } = string.Empty;
}