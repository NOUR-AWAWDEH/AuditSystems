using Ardalis.Result;
using AuditSystem.Application.Features.Common.Pagination;
using AuditSystem.Contract.Interfaces.ModelServices.JobsServices;
using AuditSystem.Contract.Models.Jobs;
using AuditSystem.Contract.Models.Pagination;
using MediatR;

namespace AuditSystem.Application.Features.Jobs.JobPrioritization.Search;

internal sealed class SearchJobPrioritizationQueryHandler(IJobPrioritizationService jobPrioritizationService) 
    : IRequestHandler<SearchJobPrioritizationQuery, Result<PaginationRequestResponse<JobPrioritizationModel>>>
{
    public async Task<Result<PaginationRequestResponse<JobPrioritizationModel>>> Handle(SearchJobPrioritizationQuery request, CancellationToken cancellationToken)
    {
        var page = new Page(request.PageNumber, request.PageSize);
        var searchResult = await jobPrioritizationService.SearchAsync(request.SearchTerm, page);
        var response = new PaginationRequestResponse<JobPrioritizationModel>(searchResult.Items, searchResult.TotalCount);
        return Result.Success(response);
    }
}