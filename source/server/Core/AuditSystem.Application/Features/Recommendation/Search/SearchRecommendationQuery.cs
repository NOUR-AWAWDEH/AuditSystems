using AuditSystem.Application.Features.Common.Pagination;
using AuditSystem.Contract.Interfaces;
using AuditSystem.Contract.Models.Recommendation;

namespace AuditSystem.Application.Features.Recommendation.Search;

public sealed record SearchRecommendationQuery(
    string? SearchTerm,
    Guid ObservationId
) : PaginationRequest<RecommendationModel>, IUserAwareRequest
{
    public Guid UserId { get; set; }
}