using Ardalis.Result;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using AuditSystem.Contract.Interfaces.ModelServices.RecommendationServices;
using AuditSystem.Contract.Models.Recommendation;

namespace AuditSystem.Application.Features.Recommendation.Create;

internal sealed class CreateRecommendationCommandHandler(
    IRecommendationService recommendationService,
    IMapper mapper,
    ILogger<CreateRecommendationCommandHandler> logger)
    : IRequestHandler<CreateRecommendationCommand, Result<CreateRecommendationCommandResponse>>
{
    public async Task<Result<CreateRecommendationCommandResponse>> Handle(CreateRecommendationCommand request, CancellationToken cancellationToken)
    {
        var recommendationModel = mapper.Map<RecommendationModel>(request);
        var recommendationId = await recommendationService.CreateRecommendationAsync(recommendationModel, request.UserId);
        
        logger.LogInformation("Recommendation with Title {RecommendationTitle} has been created by user with id: {UserId}", request.Title, request.UserId);
        return Result<CreateRecommendationCommandResponse>.Created(new(recommendationId), "Recommendation has been created.");
    }
}