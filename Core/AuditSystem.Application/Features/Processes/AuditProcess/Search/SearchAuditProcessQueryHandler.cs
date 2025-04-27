using Ardalis.Result;
using AuditSystem.Application.Features.Common.Pagination;
using AuditSystem.Contract.Interfaces.ModelServices.ProcessesServices;
using AuditSystem.Contract.Models.Pagination;
using AuditSystem.Contract.Models.Processes;
using MediatR;

namespace AuditSystem.Application.Features.Processes.AuditProcess.Search;

internal sealed class SearchAuditProcessQueryHandler(IAuditProcessService auditProcessService) 
    : IRequestHandler<SearchAuditProcessQuery, Result<PaginationRequestResponse<AuditProcessModel>>>
{
    public async Task<Result<PaginationRequestResponse<AuditProcessModel>>> Handle(SearchAuditProcessQuery request, CancellationToken cancellationToken)
    {
        var page = new Page(request.PageNumber, request.PageSize);
        var searchResult = await auditProcessService.SearchAsync(request.SearchTerm, page);
        var response = new PaginationRequestResponse<AuditProcessModel>(searchResult.Items, searchResult.TotalCount);
        return Result.Success(response);
    }
}
