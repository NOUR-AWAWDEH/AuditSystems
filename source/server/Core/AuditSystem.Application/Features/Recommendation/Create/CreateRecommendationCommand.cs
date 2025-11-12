using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.Recommendation.Create;

public sealed record CreateRecommendationCommand(
    string Title,
    string Description,
    Guid ObservationId
) : UserAwareCommand<Result<CreateRecommendationCommandResponse>>;
