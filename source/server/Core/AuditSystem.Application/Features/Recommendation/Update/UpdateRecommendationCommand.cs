using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.Recommendation.Update;

public sealed record UpdateRecommendationCommand(
    Guid Id,
    string Description,
    string Title,
    Guid ObservationId
) : UserAwareCommand<Result>;