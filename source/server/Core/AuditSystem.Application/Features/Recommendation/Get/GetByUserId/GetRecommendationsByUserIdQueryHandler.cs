using Ardalis.Result;
using AuditSystem.Contract.Interfaces.ModelServices.RecommendationServices;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AuditSystem.Application.Features.Recommendation.Get.GetByUserId;

internal sealed class GetRecommendationsByUserIdQueryHandler(
    IRecommendationService recommendationService,
    ILogger<GetRecommendationsByUserIdQueryHandler> logger ) : 
    IRequestHandler<GetRecommendationsByUserIdQuery, Result<GetRecommendationsByUserIdQueryResponse>>
{
    public async Task<Result<GetRecommendationsByUserIdQueryResponse>> Handle(      
        GetRecommendationsByUserIdQuery request, 
        CancellationToken cancellationToken)
    {
        var recommendations = await recommendationService.GetRecommendationsByUserIdAsync(request.UserId);
        logger.LogDebug("Recommendations retrieved successfully for user {UserId}", request.UserId);
        return Result<GetRecommendationsByUserIdQueryResponse>.Success(
            new(recommendations),
            "Recommendations retrieved successfully");
    }
}
