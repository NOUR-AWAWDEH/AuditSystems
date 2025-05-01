using Ardalis.Result;
using AuditSystem.Application.Features.Common.Pagination;
using AuditSystem.Contract.Interfaces.ModelServices.TasksServices;
using AuditSystem.Contract.Models.Pagination;
using AuditSystem.Contract.Models.Tasks;
using MediatR;

namespace AuditSystem.Application.Features.Tasks.Search;

internal sealed class SearchTaskManagementQueryHandler(ITaskManagementService taskManagementService) :
    IRequestHandler<SearchTaskManagementQuery, Result<PaginationRequestResponse<TaskManagementModel>>>
{
    public async Task<Result<PaginationRequestResponse<TaskManagementModel>>> Handle(SearchTaskManagementQuery request, CancellationToken cancellationToken)
    {
        var page = new Page(request.PageNumber, request.PageSize);
        var searchResult = await taskManagementService.SearchAsync(request.SearchTerm, page);
        var response = new PaginationRequestResponse<TaskManagementModel>(searchResult.Items, searchResult.TotalCount);
        return Result.Success(response);
    }
}