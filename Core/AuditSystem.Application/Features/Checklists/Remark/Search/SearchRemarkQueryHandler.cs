using Ardalis.Result;
using AuditSystem.Application.Features.Common.Pagination;
using AuditSystem.Contract.Interfaces.ModelServices.ChecklistServices;
using AuditSystem.Contract.Models.Checklists;
using AuditSystem.Contract.Models.Pagination;
using MediatR;

namespace AuditSystem.Application.Features.Checklists.Remark.Search;

internal sealed class SearchRemarkQueryHandler(IRemarkService remarkService)
    : IRequestHandler<SearchRemarkQuery, Result<PaginationRequestResponse<RemarkModel>>>
{
    public async Task<Result<PaginationRequestResponse<RemarkModel>>> Handle(SearchRemarkQuery request, CancellationToken cancellationToken)
    {
        var page = new Page(request.PageNumber, request.PageSize);
        var searchResuly = await remarkService.SearchAsync(request.SearchTerm, page);
        var response = new PaginationRequestResponse<RemarkModel>(searchResuly.Items, searchResuly.TotalCount);
        return Result.Success(response);
    }
}