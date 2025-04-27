using Ardalis.Result;
using AuditSystem.Application.Features.Common.Pagination;
using AuditSystem.Contract.Interfaces.ModelServices.JobsServices;
using AuditSystem.Contract.Models.Jobs;
using AuditSystem.Contract.Models.Pagination;
using MediatR;

namespace AuditSystem.Application.Features.Jobs.AuditJob.Search;

internal sealed class SearchAuditJobQueryHandler(IAuditJobService auditJobService) 
    : IRequestHandler<SearchAuditJobQuery, Result<PaginationRequestResponse<AuditJobModel>>>
{
    public async Task<Result<PaginationRequestResponse<AuditJobModel>>> Handle(SearchAuditJobQuery request, CancellationToken cancellationToken)
    {
        var page = new Page(request.PageNumber, request.PageSize);
        var searchResult = await auditJobService.SearchAsync(request.SearchTerm, page);
        var response = new PaginationRequestResponse<AuditJobModel>(searchResult.Items, searchResult.TotalCount);
        return Result.Success(response);
    }
}