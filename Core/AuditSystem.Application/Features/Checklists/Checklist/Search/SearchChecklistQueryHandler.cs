using Ardalis.Result;
using AuditSystem.Application.Features.Common.Pagination;
using AuditSystem.Contract.Interfaces.ModelServices.ChecklistServices;
using AuditSystem.Contract.Models.Checklists;
using AuditSystem.Contract.Models.Pagination;
using MediatR;

namespace AuditSystem.Application.Features.Checklists.Checklist.Search;

internal sealed class SearchChecklistQueryHandler(IChecklistService checklistService)
    : IRequestHandler<SearchChecklistQuery, Result<PaginationRequestResponse<ChecklistModel>>>
{
    public async Task<Result<PaginationRequestResponse<ChecklistModel>>> Handle(SearchChecklistQuery request, CancellationToken cancellationToken)
    {
        var page = new Page(request.PageNumber, request.PageSize);
        var searchResult =  await checklistService.SearchAsync(request.SearchTerm, page);
        var response = new PaginationRequestResponse<ChecklistModel>(searchResult.Items, searchResult.TotalCount);
        return Result.Success(response);
    }
}