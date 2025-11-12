using MediatR;
using Ardalis.Result;
using Microsoft.Extensions.Logging;
using AuditSystem.Contract.Interfaces.ModelServices.RecommendationServices;


namespace AuditSystem.Application.Features.Recommendation.Get.GetById;

internal sealed class GetRecommendationByIdQueryHandler(
    IRecommendationService recommendationService,
    ILogger<GetRecommendationByIdQueryHandler> logger) 
    : IRequestHandler<GetRecommendationByIdQuery, Result<GetRecommendationByIdQueryResponse>>
{
    public async Task<Result<GetRecommendationByIdQueryResponse>> Handle(GetRecommendationByIdQuery request, CancellationToken cancellationToken)
    {
        var recommendation = await recommendationService.GetRecommendationByIdAsync(request.Id, request.UserId);
        
        logger.LogInformation("Recommendation with id: {RecommendationId} has been retrieved successfully by user with id: {UserId}", request.Id, request.UserId);
        return Result<GetRecommendationByIdQueryResponse>.Success(new GetRecommendationByIdQueryResponse(recommendation), "Recommendation retrieved successfully");
    }
}