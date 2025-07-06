using Ardalis.Result;
using AuditSystem.Application.Features.Common.Pagination;
using AuditSystem.Contract.Interfaces.ModelServices.AuditServices;
using AuditSystem.Contract.Models.Audit;
using AuditSystem.Contract.Models.Pagination;
using MediatR;

namespace AuditSystem.Application.Features.Audit.AuditEngagement.Search;

internal sealed class SearchAuditEngagementQueryHandler(IAuditEngagementService auditEngagementService)
    : IRequestHandler<SearchAuditEngagementQuery, Result<PaginationRequestResponse<AuditEngagementModel>>>
{
    public async Task<Result<PaginationRequestResponse<AuditEngagementModel>>> Handle(SearchAuditEngagementQuery request, CancellationToken cancellationToken)
    {
        var page = new Page(request.PageNumber, request.PageSize);
        var searchResult = await auditEngagementService.SearchAsync(request.SearchTerm, page);
        var response = new PaginationRequestResponse<AuditEngagementModel>(searchResult.Items, searchResult.TotalCount);
        return Result.Success(response);
    }
}
