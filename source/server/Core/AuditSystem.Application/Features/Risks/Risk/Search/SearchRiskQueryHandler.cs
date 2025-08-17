using Ardalis.Result;
using AuditSystem.Application.Features.Common.Pagination;
using AuditSystem.Contract.Interfaces.ModelServices.RisksServices;
using AuditSystem.Contract.Models.Pagination;
using AuditSystem.Contract.Models.Risks;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AuditSystem.Application.Features.Risks.Risk.Search;

internal sealed class SearchRiskQueryHandler(
    IRiskService riskService,
    ILogger<SearchRiskQueryHandler> logger) :
    IRequestHandler<SearchRiskQuery, Result<PaginationRequestResponse<RiskModel>>>
{
    public async Task<Result<PaginationRequestResponse<RiskModel>>> Handle(
        SearchRiskQuery request, 
        CancellationToken cancellationToken)
    {
        var page = new Page(request.PageNumber, request.PageSize);
        var searchResult = await riskService.SearchByUserIdAsync(request.UserId,request.SearchTerm, request.ObjectiveId, page);

        logger.LogInformation("Risks with user id: {UserId} was retrieved", request.UserId);
        var response = new PaginationRequestResponse<RiskModel>(searchResult.Items, searchResult.TotalCount);
        return Result.Success(response);
    }
}