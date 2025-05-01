using Ardalis.Result;
using AuditSystem.Application.Features.Common.Pagination;
using AuditSystem.Contract.Interfaces.ModelServices.RiskControlsServices;
using AuditSystem.Contract.Models.Pagination;
using AuditSystem.Contract.Models.RiskControls;
using MediatR;

namespace AuditSystem.Application.Features.RiskControls.RiskControlMatrix.Search;

internal sealed class SearchRiskControlMatrixQueryHandler(IRiskControlMatrixService riskControlMatrixService) :
    IRequestHandler<SearchRiskControlMatrixQuery, Result<PaginationRequestResponse<RiskControlMatrixModel>>>
{
    public async Task<Result<PaginationRequestResponse<RiskControlMatrixModel>>> Handle(SearchRiskControlMatrixQuery request, CancellationToken cancellationToken)
    {
        var page = new Page(request.PageNumber, request.PageSize);
        var searchResult = await riskControlMatrixService.SearchAsync(request.SearchTerm, page);
        var response = new PaginationRequestResponse<RiskControlMatrixModel>(searchResult.Items, searchResult.TotalCount);
        return Result.Success(response);
    }
}