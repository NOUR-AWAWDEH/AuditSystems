using Ardalis.Result;
using AuditSystem.Application.Features.Common.Pagination;
using AuditSystem.Contract.Interfaces.ModelServices.AuditServices;
using AuditSystem.Contract.Models.Audit;
using AuditSystem.Contract.Models.Pagination;
using MediatR;

namespace AuditSystem.Application.Features.Audit.BusinessObjective.Search;

internal sealed class SearchBusinessObjectiveQueryHandler(IBusinessObjectiveService businessObjectiveService) 
    : IRequestHandler<SearchBusinessObjectiveQuery, Result<PaginationRequestResponse<BusinessObjectiveModel>>>
{
    public async Task<Result<PaginationRequestResponse<BusinessObjectiveModel>>> Handle(SearchBusinessObjectiveQuery request, CancellationToken cancellationToken)
    {
        var page = new Page(request.PageNumber, request.PageSize);
        var searchResult = await businessObjectiveService.SearchAsync(request.SearchTerm, page);
        var response = new PaginationRequestResponse<BusinessObjectiveModel>(searchResult.Items, searchResult.TotalCount);
        return Result.Success(response);
    }
}