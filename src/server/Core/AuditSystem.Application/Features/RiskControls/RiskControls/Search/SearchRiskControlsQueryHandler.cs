using Ardalis.Result;
using AuditSystem.Application.Features.Common.Pagination;
using AuditSystem.Contract.Interfaces.ModelServices.RiskControlsServices;
using AuditSystem.Contract.Models.Pagination;
using AuditSystem.Contract.Models.RiskControls;
using MediatR;

namespace AuditSystem.Application.Features.RiskControls.RiskControls.Search;

internal sealed class SearchRiskControlsQueryHandler(IRiskControlService riskControlService) 
: IRequestHandler<SearchRiskControlsQuery, Result<PaginationRequestResponse<RiskControlsModel>>>
{
    public async Task<Result<PaginationRequestResponse<RiskControlsModel>>> Handle(SearchRiskControlsQuery request, CancellationToken cancellationToken)
    {
        var page = new Page(request.PageNumber, request.PageSize);
        var searchResult = await riskControlService.SearchAsync(request.SearchTerm, page);
        var response = new PaginationRequestResponse<RiskControlsModel>(searchResult.Items, searchResult.TotalCount);
        return Result.Success(response);
    }
}