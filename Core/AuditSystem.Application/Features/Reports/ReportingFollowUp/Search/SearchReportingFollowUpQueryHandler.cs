using Ardalis.Result;
using AuditSystem.Application.Features.Common.Pagination;
using AuditSystem.Contract.Interfaces.ModelServices.ReportsServices;
using AuditSystem.Contract.Models.Pagination;
using AuditSystem.Contract.Models.Reports;
using MediatR;

namespace AuditSystem.Application.Features.Reports.ReportingFollowUp.Search;

internal sealed class SearchReportingFollowUpQueryHandler(IReportingFollowUpService reportingFollowUpService)
    : IRequestHandler<SearchReportingFollowUpQuery, Result<PaginationRequestResponse<ReportingFollowUpModel>>>
{
    public async Task<Result<PaginationRequestResponse<ReportingFollowUpModel>>> Handle(SearchReportingFollowUpQuery request, CancellationToken cancellationToken)
    {
        var page = new Page(request.PageNumber, request.PageSize);
        var searchResult = await reportingFollowUpService.SearchAsync(request.SearchTerm, page);
        var response = new PaginationRequestResponse<ReportingFollowUpModel>(searchResult.Items, searchResult.TotalCount);
        return Result.Success(response);
    }
}