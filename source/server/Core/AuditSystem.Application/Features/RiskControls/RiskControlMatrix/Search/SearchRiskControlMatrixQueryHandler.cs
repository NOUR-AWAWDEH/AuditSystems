using Ardalis.Result;
using AuditSystem.Application.Features.Common.Pagination;
using AuditSystem.Contract.Interfaces.ModelServices.RiskControlsServices;
using AuditSystem.Contract.Interfaces.ModelServices.RiskControlsServices.RiskControlMatrixServices;
using AuditSystem.Contract.Models.Pagination;
using AuditSystem.Contract.Models.RiskControls;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AuditSystem.Application.Features.RiskControls.RiskControlMatrix.Search;

internal sealed class SearchRiskControlMatrixQueryHandler(
    IRiskControlMatrixService riskControlMatrixService,
    ILogger<SearchRiskControlMatrixQueryHandler> logger)
    : IRequestHandler<SearchRiskControlMatrixQuery, Result<PaginationRequestResponse<RiskControlMatrixModel>>>
{
    public async Task<Result<PaginationRequestResponse<RiskControlMatrixModel>>> Handle(SearchRiskControlMatrixQuery request, CancellationToken cancellationToken)
    {
        var page = new Page(request.PageNumber, request.PageSize);
        var searchResult = await riskControlMatrixService.SearchByUserIdAsync(request.UserId, request.SearchTerm, request.SubProcessId, page);
        logger.LogInformation("Risk control matrices with search term: {SearchTerm} and page: {Page} was retrieved", request.SearchTerm, page);
        var response = new PaginationRequestResponse<RiskControlMatrixModel>(searchResult.Items, searchResult.TotalCount);
        return Result.Success(response, "Risk control matrices retrieved successfully");
    }
}