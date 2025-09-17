using Ardalis.Result;
using AuditSystem.Application.Features.Common.Pagination;
using AuditSystem.Contract.Interfaces.ModelServices.OrganizationServices.SubLocationServices;

using AuditSystem.Contract.Models.Organization;
using AuditSystem.Contract.Models.Pagination;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AuditSystem.Application.Features.Organization.SubLocation.Search;

internal sealed class SearchSubLocationQueryHandler(
    ISubLocationService subLocationService,
    ILogger<SearchSubLocationQueryHandler> logger) 
    : IRequestHandler<SearchSubLocationQuery, Result<PaginationRequestResponse<SubLocationModel>>>
{
    public async Task<Result<PaginationRequestResponse<SubLocationModel>>> Handle(SearchSubLocationQuery request, CancellationToken cancellationToken)
    {
        var page = new Page(request.PageNumber, request.PageSize);

        var searchResult = await subLocationService.SearchByUserIdAsync(request.UserId, request.SearchTerm, request.LocationId, page);
        logger.LogInformation("Sub-locations search result retrieved successfully for user {UserId}", request.UserId);
        
        var response = new PaginationRequestResponse<SubLocationModel>(searchResult.Items, searchResult.TotalCount);
        return Result.Success(response);
    }
}