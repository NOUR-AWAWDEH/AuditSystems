using Ardalis.Result;
using AuditSystem.Application.Features.Common.Pagination;
using AuditSystem.Contract.Interfaces.ModelServices.OrganizationServices.JobTitleServices;
using AuditSystem.Contract.Models.Organization;
using AuditSystem.Contract.Models.Pagination;
using MediatR;

namespace AuditSystem.Application.Features.Organization.JobTitle.Search;

internal sealed class SearchJobTitleQueryHandler(IJobTitleService jobTitleService)
    : IRequestHandler<SearchJobTitleQuery, Result<PaginationRequestResponse<JobTitleModel>>>
{
    public async Task<Result<PaginationRequestResponse<JobTitleModel>>> Handle(SearchJobTitleQuery request, CancellationToken cancellationToken)
    {
        var page = new Page(request.PageNumber, request.PageSize);
        var searchResult = await jobTitleService.SearchByCompanyIdAsync(request.SearchTerm, request.CompanyId, page, request.UserId);
        var response = new PaginationRequestResponse<JobTitleModel>(searchResult.Items, searchResult.TotalCount);
        return Result.Success(response);
    }
}
