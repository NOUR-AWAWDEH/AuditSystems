using Ardalis.Result;
using AuditSystem.Contract.Interfaces.ModelServices.RecommendationServices;
using AuditSystem.Contract.Models.Recommendation;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AuditSystem.Application.Features.Recommendation.Update;

public sealed class UpdateRecommendationCommandHandler(
    IRecommendationService recommendationService,
    IMapper mapper,
    ILogger<UpdateRecommendationCommandHandler> logger)
    : IRequestHandler<UpdateRecommendationCommand, Result>
{
    public async Task<Result> Handle(UpdateRecommendationCommand request, CancellationToken cancellationToken)
    {
        var recommendationModel = mapper.Map<RecommendationModel>(request);
        await recommendationService.UpdateRecommendationAsync(recommendationModel, request.UserId);

        logger.LogInformation("Recommendation with ID: {RecommendationId} updated by User Id: {UserId}", request.Id, request.UserId);
        return Result.SuccessWithMessage("Recommendation updated successfully");
    }
}
