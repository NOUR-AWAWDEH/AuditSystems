using Ardalis.Result;
using AuditSystem.Application.Features.Common.Pagination;
using AuditSystem.Contract.Interfaces.ModelServices.RiskControlsServices.ObjectiveServices;
using AuditSystem.Contract.Models.Pagination;
using AuditSystem.Contract.Models.RiskControls;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AuditSystem.Application.Features.RiskControls.Objective.Search;

internal sealed class SearchObjectiveQueryHandler(
    IObjectiveService objectiveService,
    ILogger<SearchObjectiveQueryHandler> logger) 
    : IRequestHandler<SearchObjectiveQuery, Result<PaginationRequestResponse<ObjectiveModel>>>
{
    public async Task<Result<PaginationRequestResponse<ObjectiveModel>>> Handle(
        SearchObjectiveQuery request,
        CancellationToken cancellationToken)
    {
        var page = new Page(request.PageNumber, request.PageSize);
        var searchResult = await objectiveService.SearchByUserIdAsync(request.UserId ,request.SearchTerm, request.RiskControlMatrixId, page);
        logger.LogInformation("Search objectives by user id {UserId} with search term {SearchTerm} and risk control matrix id {RiskControlMatrixId} and page {PageNumber} and page size {PageSize}", request.UserId, request.SearchTerm, request.RiskControlMatrixId, request.PageNumber, request.PageSize);
        var response = new PaginationRequestResponse<ObjectiveModel>(searchResult.Items, searchResult.TotalCount);
        return Result.Success(response, "Objectives searched successfully");
    }
}