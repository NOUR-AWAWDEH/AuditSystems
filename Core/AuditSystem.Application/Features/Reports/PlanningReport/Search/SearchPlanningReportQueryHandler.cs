using Ardalis.Result;
using AuditSystem.Application.Features.Common.Pagination;
using AuditSystem.Contract.Interfaces.ModelServices.ReportsServices;
using AuditSystem.Contract.Models.Pagination;
using AuditSystem.Contract.Models.Reports;
using MediatR;

namespace AuditSystem.Application.Features.Reports.PlanningReport.Search;

internal sealed class SearchPlanningReportQueryHandler(IPlanningReportService planningReportService) 
    : IRequestHandler<SearchPlanningReportQuery, Result<PaginationRequestResponse<PlanningReportModel>>>
{
    public async Task<Result<PaginationRequestResponse<PlanningReportModel>>> Handle(SearchPlanningReportQuery request, CancellationToken cancellationToken)
    {
        var page = new Page(request.PageNumber, request.PageSize);
        var searchResult = await planningReportService.SearchAsync(request.SearchTerm, page);
        var response = new PaginationRequestResponse<PlanningReportModel>(searchResult.Items, searchResult.TotalCount);
        return Result.Success(response);
    }
}