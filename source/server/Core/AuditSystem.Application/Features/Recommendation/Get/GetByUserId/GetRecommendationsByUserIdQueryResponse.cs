using AuditSystem.Contract.Models.Recommendation;

namespace AuditSystem.Application.Features.Recommendation.Get.GetByUserId;

public sealed record GetRecommendationsByUserIdQueryResponse(IReadOnlyList<RecommendationModel> Recommendations);
