using Ardalis.Result;
using AuditSystem.Application.Features.Common.Pagination;
using AuditSystem.Contract.Interfaces.ModelServices.ReportsServices;
using AuditSystem.Contract.Models.Pagination;
using AuditSystem.Contract.Models.Reports;
using MediatR;

namespace AuditSystem.Application.Features.Reports.AuditPlanSummaryReport.Search;

internal sealed class SearchAuditPlanSummaryReportQueryHandler(IAuditPlanSummaryReportService auditPlanSummaryReportService) 
    : IRequestHandler<SearchAuditPlanSummaryReportQuery, Result<PaginationRequestResponse<AuditPlanSummaryReportModel>>>
{
    public async Task<Result<PaginationRequestResponse<AuditPlanSummaryReportModel>>> Handle(SearchAuditPlanSummaryReportQuery request, CancellationToken cancellationToken)
    {
        var page = new Page(request.PageNumber, request.PageSize);
        var searchResult = await auditPlanSummaryReportService.SearchAsync(request.SearchTerm, page);
        var response = new PaginationRequestResponse<AuditPlanSummaryReportModel>(searchResult.Items, searchResult.TotalCount);
        return Result.Success(response);
    }
}