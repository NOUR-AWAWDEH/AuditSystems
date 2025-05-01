using Ardalis.Result;
using AuditSystem.Application.Features.Common.Pagination;
using AuditSystem.Contract.Interfaces.ModelServices.ReportsServices;
using AuditSystem.Contract.Models.Pagination;
using AuditSystem.Contract.Models.Reports;
using MediatR;

namespace AuditSystem.Application.Features.Reports.JobTimeAllocationReport.Search;

internal sealed class SearchJobTimeAllocationReportQueryHandler(IJobTimeAllocationReportService jobTimeAllocationReportService) 
    : IRequestHandler<SearchJobTimeAllocationReportQuery, Result<PaginationRequestResponse<JobTimeAllocationReportModel>>>
{
    public async Task<Result<PaginationRequestResponse<JobTimeAllocationReportModel>>> Handle(SearchJobTimeAllocationReportQuery request, CancellationToken cancellationToken)
    {
        var page = new Page(request.PageNumber, request.PageSize);
        var searchResult = await jobTimeAllocationReportService.SearchAsync(request.SearchTerm, page);
        var response = new PaginationRequestResponse<JobTimeAllocationReportModel>(searchResult.Items, searchResult.TotalCount);
        return Result.Success(response);
    }
}