using Microsoft.Extensions.Logging;
using MediatR;
using Ardalis.Result;
using AuditSystem.Contract.Interfaces.ModelServices.RecommendationServices;

namespace AuditSystem.Application.Features.Recommendation.Delete;

internal sealed class DeleteRecommendationCommandHandler(
    IRecommendationService recommendationService,
    ILogger<DeleteRecommendationCommandHandler> logger) :
    IRequestHandler<DeleteRecommendationCommand, Result>
{
    public async Task<Result> Handle(DeleteRecommendationCommand request, CancellationToken cancellationToken)
    {
        await recommendationService.DeleteRecommendationAsync(request.Id, request.UserId);
        logger.LogInformation("Recommendation with ID {Id} has been deleted by user with id: {UserId}", request.Id, request.UserId);
        return Result.SuccessWithMessage("Recommendation deleted successfully");
    }
}