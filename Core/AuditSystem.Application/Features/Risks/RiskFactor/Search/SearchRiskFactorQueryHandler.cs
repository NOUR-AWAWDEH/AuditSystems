using Ardalis.Result;
using AuditSystem.Application.Features.Common.Pagination;
using AuditSystem.Contract.Interfaces.ModelServices.RisksServices;
using AuditSystem.Contract.Models.Pagination;
using AuditSystem.Contract.Models.Risks;
using MediatR;

namespace AuditSystem.Application.Features.Risks.RiskFactor.Search;

internal sealed class SearchRiskFactorQueryHandler(IRiskFactorService riskFactorService) :
    IRequestHandler<SearchRiskFactorQuery, Result<PaginationRequestResponse<RiskFactorModel>>>
{
    public async Task<Result<PaginationRequestResponse<RiskFactorModel>>> Handle(SearchRiskFactorQuery request, CancellationToken cancellationToken)
    {
        var page = new Page(request.PageNumber, request.PageSize);
        var searchResult = await riskFactorService.SearchAsync(request.SearchTerm, page);
        var response = new PaginationRequestResponse<RiskFactorModel>(searchResult.Items, searchResult.TotalCount);
        return Result.Success(response);
    }
}