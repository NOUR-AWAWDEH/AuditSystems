using AuditSystem.Application.Features.Common.Pagination;
using AuditSystem.Contract.Models.Risks;

namespace AuditSystem.Application.Features.Risks.RiskAssessment.Search;

public sealed record class SearchRiskAssessmentQuery : PaginationRequest<RiskAssessmentModel>
{
    public string? SearchTerm { get; set; } = string.Empty;
}