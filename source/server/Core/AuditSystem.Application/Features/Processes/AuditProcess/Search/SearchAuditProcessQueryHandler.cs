using Ardalis.Result;
using AuditSystem.Application.Features.Common.Pagination;
using AuditSystem.Contract.Interfaces.ModelServices.ProcessServices.AuditProcessServices;
using AuditSystem.Contract.Models.Pagination;
using AuditSystem.Contract.Models.Processes;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AuditSystem.Application.Features.Processes.AuditProcess.Search;

internal sealed class SearchAuditProcessQueryHandler(
    IAuditProcessService auditProcessService,
    ILogger<SearchAuditProcessQueryHandler> logger) 
    : IRequestHandler<SearchAuditProcessQuery, Result<PaginationRequestResponse<AuditProcessModel>>>
{
    public async Task<Result<PaginationRequestResponse<AuditProcessModel>>> Handle(
        SearchAuditProcessQuery request, CancellationToken cancellationToken)
    {
        var page = new Page(request.PageNumber, request.PageSize);
        var searchResult = await auditProcessService.SearchByUserIdAsync(request.UserId, request.SearchTerm, request.SubDepartmentId, page);
        
        logger.LogInformation("Audit processes with user id: {UserId} was retrieved", request.UserId);
        var response = new PaginationRequestResponse<AuditProcessModel>(searchResult.Items, searchResult.TotalCount);
        return Result.Success(response);
    }
}
