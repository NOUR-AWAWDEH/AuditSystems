using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.Recommendation.Get.GetById;

public sealed record GetRecommendationByIdQuery(
    Guid Id
) : UserAwareQuery<Result<GetRecommendationByIdQueryResponse>>;
