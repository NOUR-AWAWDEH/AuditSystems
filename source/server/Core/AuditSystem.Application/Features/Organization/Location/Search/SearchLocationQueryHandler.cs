using Ardalis.Result;
using AuditSystem.Application.Features.Common.Pagination;
using AuditSystem.Contract.Interfaces.ModelServices.OrganizationServices.LocationServices;

using AuditSystem.Contract.Models.Organization;
using AuditSystem.Contract.Models.Pagination;
using MediatR;

namespace AuditSystem.Application.Features.Organization.Location.Search;

internal sealed class SearchLocationQueryHandler(ILocationService locationService) 
    : IRequestHandler<SearchLocationQuery, Result<PaginationRequestResponse<LocationModel>>>
{
    public async Task<Result<PaginationRequestResponse<LocationModel>>> Handle(SearchLocationQuery request, CancellationToken cancellationToken)
    {
        var page = new Page(request.PageNumber, request.PageSize);
        // Add user filtering to the search
        var searchResult = await locationService.SearchByUserIdAsync(request.UserId, request.SearchTerm, page);
        var response = new PaginationRequestResponse<LocationModel>(searchResult.Items, searchResult.TotalCount);
        return Result.Success(response);
    }
}