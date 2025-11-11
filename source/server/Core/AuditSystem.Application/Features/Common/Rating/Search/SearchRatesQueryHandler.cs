using Ardalis.Result;
using AuditSystem.Application.Features.Common.Pagination;
using AuditSystem.Contract.Interfaces.ModelServices.CommonServices;
using AuditSystem.Contract.Models.Common;
using AuditSystem.Contract.Models.Pagination;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AuditSystem.Application.Features.Common.Rating.Search;

internal sealed class SearchRatesQueryHandler(
    IRateService rateService,
    ILogger<SearchRatesQueryHandler> logger) : 
    IRequestHandler<SearchRatesQuery, Result<PaginationRequestResponse<RateModel>>>
{
    public async Task<Result<PaginationRequestResponse<RateModel>>> Handle(
        SearchRatesQuery request, 
        CancellationToken cancellationToken
    )
    {
        var page = new Page(request.PageNumber, request.PageSize);
        var searchResult = await rateService.SearchAllAsync(page);

        logger.LogInformation("Found {Count} rates for filter '{SearchTerm}'", searchResult.Items.Count, request.SearchTerm);
        var response = new PaginationRequestResponse<RateModel>(searchResult.Items, searchResult.TotalCount);
        return Result.Success(response);
    }
}
