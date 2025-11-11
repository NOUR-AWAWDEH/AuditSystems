using Ardalis.Result;
using AuditSystem.Application.Features.Common.Pagination;
using AuditSystem.Contract.Interfaces.ModelServices.RisksServices.ControlDesignServices;
using AuditSystem.Contract.Models.Pagination;
using AuditSystem.Contract.Models.Risks;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AuditSystem.Application.Features.Risks.ControlDesign.Search;

internal sealed class SearchControlDesignQueryHandler(
    IControlDesignService controlDesignService,
    ILogger<SearchControlDesignQueryHandler> logger) :
    IRequestHandler<SearchControlDesignQuery, Result<PaginationRequestResponse<ControlDesignModel>>>
{
    public async Task<Result<PaginationRequestResponse<ControlDesignModel>>> Handle(
        SearchControlDesignQuery request, 
        CancellationToken cancellationToken)
    {
        var page = new Page(request.PageNumber, request.PageSize);

        var searchResult = await controlDesignService.SearchByUserIdAsync(
            request.UserId, 
            request.SearchTerm, 
            request.ExitControlId, 
            page);

        logger.LogInformation(
            "Control designs with user id: {UserId} retrieved, count: {Count}", 
            request.UserId, 
            searchResult.TotalCount);

        var response = new PaginationRequestResponse<ControlDesignModel>(
            searchResult.Items, 
            searchResult.TotalCount);

        return Result.Success(response);
    }
}