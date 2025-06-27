using Ardalis.Result;
using AuditSystem.Application.Features.Common.Pagination;
using AuditSystem.Contract.Interfaces.ModelServices.OrganizationServices;
using AuditSystem.Contract.Models.Organization;
using AuditSystem.Contract.Models.Pagination;
using MediatR;

namespace AuditSystem.Application.Features.Organization.SubLocation.Search;

internal sealed class SearchSubLocationQueryHandler(ISubLocationService subLocationService) 
    : IRequestHandler<SearchSubLocationQuery, Result<PaginationRequestResponse<SubLocationModel>>>
{
    public async Task<Result<PaginationRequestResponse<SubLocationModel>>> Handle(SearchSubLocationQuery request, CancellationToken cancellationToken)
    {
        var page = new Page(request.PageNumber, request.PageSize);
        // Add user filtering to the search
        var searchResult = await subLocationService.SearchByUserIdAsync(request.UserId, request.SearchTerm, page);
        var response = new PaginationRequestResponse<SubLocationModel>(searchResult.Items, searchResult.TotalCount);
        return Result.Success(response);
    }
}