using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.Recommendation.Get.GetByUserId;

public sealed record GetRecommendationsByUserIdQuery : UserAwareQuery<Result<GetRecommendationsByUserIdQueryResponse>>;
