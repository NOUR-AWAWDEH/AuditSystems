using Ardalis.Result;
using AuditSystem.Application.Features.Common.Pagination;
using AuditSystem.Contract.Interfaces.ModelServices.AuditServices.AuditProgramServices;
using AuditSystem.Contract.Models.Audit;
using AuditSystem.Contract.Models.Pagination;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AuditSystem.Application.Features.Audit.AuditProgram.Search;

internal sealed class SearchAuditProgramQueryHandler(
    IAuditProgramService auditProgramService,
    ILogger<SearchAuditProgramQueryHandler> logger)
    : IRequestHandler<SearchAuditProgramQuery, Result<PaginationRequestResponse<AuditProgramModel>>>
{
    public async Task<Result<PaginationRequestResponse<AuditProgramModel>>> Handle(SearchAuditProgramQuery request, CancellationToken cancellationToken)
    {
        var page = new Page(request.PageNumber, request.PageSize);
        var searchResult = await auditProgramService.SearchByUserIdAsync(request.UserId, request.SearchTerm, request.ExitControlId, page);

        logger.LogInformation("Audit Programs has been retrieved successfully by user with id: {UserId}", request.UserId);
        var response = new PaginationRequestResponse<AuditProgramModel>(searchResult.Items, searchResult.TotalCount);
        return Result.Success(response);
    }
}