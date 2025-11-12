using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.Recommendation.Delete;

public sealed record DeleteRecommendationCommand(
    Guid Id
) : UserAwareCommand<Result>;