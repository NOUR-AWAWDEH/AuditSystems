using Ardalis.Result;
using AuditSystem.Application.Features.Common.Pagination;
using AuditSystem.Contract.Interfaces.ModelServices.CommonServices;
using AuditSystem.Contract.Models.Common;
using AuditSystem.Contract.Models.Pagination;
using MediatR;

namespace AuditSystem.Application.Features.Common.Rating.Search;

internal sealed class SearchRatingQueryHandler(IRatingService ratingService)
    : IRequestHandler<SearchRatingQuery, Result<PaginationRequestResponse<RatingModel>>>
{
    public async Task<Result<PaginationRequestResponse<RatingModel>>> Handle(SearchRatingQuery request, CancellationToken cancellationToken)
    {
        var page = new Page(request.PageNumber, request.PageSize);
        var searchResult = await ratingService.SearchAsync(request.SearchTerm, page);
        var response = new PaginationRequestResponse<RatingModel>(searchResult.Items, searchResult.TotalCount);
        return Result.Success(response);
    }
}