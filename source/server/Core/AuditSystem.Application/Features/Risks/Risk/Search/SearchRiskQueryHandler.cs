using Ardalis.Result;
using AuditSystem.Application.Features.Common.Pagination;
using AuditSystem.Contract.Interfaces.ModelServices.RisksServices;
using AuditSystem.Contract.Models.Pagination;
using AuditSystem.Contract.Models.Risks;
using MediatR;

namespace AuditSystem.Application.Features.Risks.Risk.Search;

internal sealed class SearchRiskQueryHandler(IRiskService riskService) :
    IRequestHandler<SearchRiskQuery, Result<PaginationRequestResponse<RiskModel>>>
{
    public async Task<Result<PaginationRequestResponse<RiskModel>>> Handle(SearchRiskQuery request, CancellationToken cancellationToken)
    {
        var page = new Page(request.PageNumber, request.PageSize);
        var searchResult = await riskService.SearchAsync(request.SearchTerm, page);
        var response = new PaginationRequestResponse<RiskModel>(searchResult.Items, searchResult.TotalCount);
        return Result.Success(response);
    }
}