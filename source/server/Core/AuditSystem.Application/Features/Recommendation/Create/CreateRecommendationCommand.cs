using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.Recommendation.Create;

public sealed record CreateRecommendationCommand(
    string Description,
    Guid ObservationId
) : UserAwareCommand<Result<CreateRecommendationCommandResponse>>;
