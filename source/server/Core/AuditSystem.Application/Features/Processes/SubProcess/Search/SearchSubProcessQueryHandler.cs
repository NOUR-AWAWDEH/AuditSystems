using Ardalis.Result;
using AuditSystem.Application.Features.Common.Pagination;
using AuditSystem.Contract.Interfaces.ModelServices.ProcessServices.SubProcessServices;
using AuditSystem.Contract.Models.Pagination;
using AuditSystem.Contract.Models.Processes;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AuditSystem.Application.Features.Processes.SubProcess.Search;

internal sealed class  SearchSubProcessQueryHandler(
    ISubProcessService subProcessService,
    ILogger<SearchSubProcessQueryHandler> logger) 
    : IRequestHandler<SearchSubProcessQuery, Result<PaginationRequestResponse<SubProcessModel>>>
{
    public async Task<Result<PaginationRequestResponse<SubProcessModel>>> Handle(
        SearchSubProcessQuery request, CancellationToken cancellationToken)
    {
        var page = new Page(request.PageNumber, request.PageSize);
        var searchResult = await subProcessService.SearchByUserIdAsync(request.UserId, request.SearchTerm, request.AuditProcessId, page);
        
        logger.LogInformation("Sub Process with user Id: {UserId} was retrieved", request.UserId);
        var response = new PaginationRequestResponse<SubProcessModel>(searchResult.Items, searchResult.TotalCount);
        return Result.Success(response, "Sub Process retrieved successfully");
    }
}