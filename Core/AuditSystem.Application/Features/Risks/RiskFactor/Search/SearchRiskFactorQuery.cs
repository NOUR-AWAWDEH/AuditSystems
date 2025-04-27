using AuditSystem.Application.Features.Common.Pagination;
using AuditSystem.Contract.Models.Risks;

namespace AuditSystem.Application.Features.Risks.RiskFactor.Search;

public sealed record class SearchRiskFactorQuery : PaginationRequest<RiskFactorModel>
{
    public string? SearchTerm { get; set; } = string.Empty;
}