using Ardalis.Result;
using AuditSystem.Application.Features.Common.Pagination;
using AuditSystem.Contract.Interfaces.ModelServices.RiskControlsServices;
using AuditSystem.Contract.Models.Pagination;
using AuditSystem.Contract.Models.RiskControls;
using MediatR;

namespace AuditSystem.Application.Features.RiskControls.RiskProgram.Search;

internal sealed class SearchRiskProgramQueryHandler(IRiskProgramService riskProgramService) :
    IRequestHandler<SearchRiskProgramQuery, Result<PaginationRequestResponse<RiskProgramModel>>>
{
    public async Task<Result<PaginationRequestResponse<RiskProgramModel>>> Handle(SearchRiskProgramQuery request, CancellationToken cancellationToken)
    {
        var page = new Page(request.PageNumber, request.PageSize);
        var searchResult = await riskProgramService.SearchAsync(request.SearchTerm, page);
        var response = new PaginationRequestResponse<RiskProgramModel>(searchResult.Items, searchResult.TotalCount);
        return Result.Success(response);
    }
}