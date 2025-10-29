using Ardalis.Result;
using AuditSystem.Application.Features.Common.Pagination;
using AuditSystem.Contract.Interfaces.ModelServices.RisksServices;
using AuditSystem.Contract.Interfaces.ModelServices.RisksServices.ExitControlServices;
using AuditSystem.Contract.Models.Pagination;
using AuditSystem.Contract.Models.Risks;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AuditSystem.Application.Features.Risks.ExitControl.Search;

internal sealed class SearchExitControlQueryHandler(
    IExitControlService exitControlService,
    ILogger<SearchExitControlQueryHandler> logger) :
    IRequestHandler<SearchExitControlQuery, Result<PaginationRequestResponse<ExitControlModel>>>
{
    public async Task<Result<PaginationRequestResponse<ExitControlModel>>> Handle(
        SearchExitControlQuery request, 
        CancellationToken cancellationToken)
    {
        var page = new Page(request.PageNumber, request.PageSize);

        var searchResult = await exitControlService.SearchByUserIdAsync(
            request.UserId, 
            request.SearchTerm, 
            request.ObjectiveId, 
            page);

        logger.LogInformation(
            "Exit controls with user id: {UserId} retrieved, count: {Count}", 
            request.UserId, 
            searchResult.TotalCount);

        var response = new PaginationRequestResponse<ExitControlModel>(
            searchResult.Items, 
            searchResult.TotalCount);

        return Result.Success(response);
    }
}