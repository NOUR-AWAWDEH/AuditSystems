using Ardalis.Result;
using AuditSystem.Application.Features.Common.Pagination;
using AuditSystem.Contract.Interfaces.ModelServices.ReportsServices;
using AuditSystem.Contract.Models.Pagination;
using AuditSystem.Contract.Models.Reports;
using MediatR;

namespace AuditSystem.Application.Features.Reports.InternalAuditConsolidationReport.Search;

internal sealed class SearchInternalAuditConsolidationReportQueryHandler(IInternalAuditConsolidationReportService internalAuditConsolidationReportService)
    : IRequestHandler<SearchInternalAuditConsolidationReportQuery, Result<PaginationRequestResponse<InternalAuditConsolidationReportModel>>>
{
    public async Task<Result<PaginationRequestResponse<InternalAuditConsolidationReportModel>>> Handle(SearchInternalAuditConsolidationReportQuery request, CancellationToken cancellationToken)
    {
        var page = new Page(request.PageNumber, request.PageSize);
        var searchResult = await internalAuditConsolidationReportService.SearchAsync(request.SearchTerm, page);
        var response = new PaginationRequestResponse<InternalAuditConsolidationReportModel>(searchResult.Items, searchResult.TotalCount);
        return Result.Success(response);
    }
}