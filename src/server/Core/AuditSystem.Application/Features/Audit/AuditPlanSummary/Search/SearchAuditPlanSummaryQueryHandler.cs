using Ardalis.Result;
using AuditSystem.Application.Features.Common.Pagination;
using AuditSystem.Contract.Interfaces.ModelServices.AuditServices;
using AuditSystem.Contract.Models.Audit;
using AuditSystem.Contract.Models.Pagination;
using MediatR;

namespace AuditSystem.Application.Features.Audit.AuditPlanSummary.Search;

internal sealed class SearchAuditPlanSummaryQueryHandler(IAuditPlanSummaryService auditPlanSummaryService) 
    : IRequestHandler<SearchAuditPlanSummaryQuery, Result<PaginationRequestResponse<AuditPlanSummaryModel>>>
{
    public async Task<Result<PaginationRequestResponse<AuditPlanSummaryModel>>> Handle(SearchAuditPlanSummaryQuery request, CancellationToken cancellationToken)
    {
        var page = new Page(request.PageNumber, request.PageSize);
        var searchResult = await auditPlanSummaryService.SearchAsync(request.SearchTerm, page);
        var response = new PaginationRequestResponse<AuditPlanSummaryModel>(searchResult.Items, searchResult.TotalCount);
        return Result.Success(response);
    }
}