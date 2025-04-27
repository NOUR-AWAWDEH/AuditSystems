using Ardalis.Result;
using AuditSystem.Application.Features.Common.Pagination;
using AuditSystem.Contract.Interfaces.ModelServices.JobsServices;
using AuditSystem.Contract.Models.Jobs;
using AuditSystem.Contract.Models.Pagination;
using MediatR;

namespace AuditSystem.Application.Features.Jobs.JobScheduling.Search;

internal sealed class SearchJobSchedulingQueryHandler(IJobSchedulingService jobSchedulingService)
    : IRequestHandler<SearchJobSchedulingQuery, Result<PaginationRequestResponse<JobSchedulingModel>>>
{
    public async Task<Result<PaginationRequestResponse<JobSchedulingModel>>> Handle(SearchJobSchedulingQuery request, CancellationToken cancellationToken)
    {
        var page = new Page(request.PageNumber, request.PageSize);
        var searchResult = await jobSchedulingService.SearchAsync(request.SearchTerm, page);
        var response = new PaginationRequestResponse<JobSchedulingModel>(searchResult.Items, searchResult.TotalCount);
        return Result.Success(response);
    }
}