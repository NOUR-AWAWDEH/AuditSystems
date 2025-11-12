using Ardalis.Result;
using AuditSystem.Application.Features.Common.Pagination;
using AuditSystem.Contract.Interfaces.ModelServices.RecommendationServices;
using AuditSystem.Contract.Models.Pagination;
using AuditSystem.Contract.Models.Recommendation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AuditSystem.Application.Features.Recommendation.Search;

internal sealed class SearchRecommendationQueryHandler(
    IRecommendationService recommendationService,
    ILogger<SearchRecommendationQueryHandler> logger)
    : IRequestHandler<SearchRecommendationQuery, Result<PaginationRequestResponse<RecommendationModel>>>
{
    public async Task<Result<PaginationRequestResponse<RecommendationModel>>> Handle(SearchRecommendationQuery request, CancellationToken cancellationToken)
    {
        var page = new Page(request.PageNumber, request.PageSize);
        var searchResult = await recommendationService.SearchByUserIdAsync(request.UserId, request.SearchTerm, request.ObservationId, page);

        logger.LogInformation("Recommendations has been retrieved successfully by observation with id: {ObservationId}", request.ObservationId);
        var response = new PaginationRequestResponse<RecommendationModel>(searchResult.Items, searchResult.TotalCount);
        return Result.Success(response, "Recommendations retrieved successfully");
    }
}