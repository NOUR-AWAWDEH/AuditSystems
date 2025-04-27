using Ardalis.Result;
using AuditSystem.Application.Features.Common.Pagination;
using AuditSystem.Contract.Interfaces.ModelServices.ReportsServices;
using AuditSystem.Contract.Models.Pagination;
using AuditSystem.Contract.Models.Reports;
using MediatR;

namespace AuditSystem.Application.Features.Reports.AuditExceptionReport.Search;

internal sealed class SearchAuditExceptionReportQueryHandler(IAuditExceptionReportService auditExceptionReportService) 
    : IRequestHandler<SearchAuditExceptionReportQuery, Result<PaginationRequestResponse<AuditExceptionReportModel>>>
{
    public async Task<Result<PaginationRequestResponse<AuditExceptionReportModel>>> Handle(SearchAuditExceptionReportQuery request, CancellationToken cancellationToken)
    {
        var page = new Page(request.PageNumber, request.PageSize);
        var searchResult = await auditExceptionReportService.SearchAsync(request.SearchTerm, page);
        var response = new PaginationRequestResponse<AuditExceptionReportModel>(searchResult.Items, searchResult.TotalCount);
        return Result.Success(response);
    }
}