using Ardalis.Result;
using AuditSystem.Application.Features.Common.Pagination;
using AuditSystem.Contract.Interfaces.ModelServices.AuditServices;
using AuditSystem.Contract.Models.Audit;
using AuditSystem.Contract.Models.Pagination;
using MediatR;

namespace AuditSystem.Application.Features.Audit.Objective.Search;

internal sealed class SearchObjectiveQueryHandler(IObjectiveService objectiveService) 
    : IRequestHandler<SearchObjectiveQuery, Result<PaginationRequestResponse<ObjectiveModel>>>
{
    public async Task<Result<PaginationRequestResponse<ObjectiveModel>>> Handle(SearchObjectiveQuery request, CancellationToken cancellationToken)
    {
        var page = new Page(request.PageNumber, request.PageSize);
        var searchResult = await objectiveService.SearchAsync(request.SearchTerm, page);
        var response = new PaginationRequestResponse<ObjectiveModel>(searchResult.Items, searchResult.TotalCount);
        return Result.Success(response);
    }
}