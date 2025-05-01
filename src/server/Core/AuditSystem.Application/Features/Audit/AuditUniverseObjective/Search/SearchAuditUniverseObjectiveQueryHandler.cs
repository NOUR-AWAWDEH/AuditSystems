using Ardalis.Result;
using AuditSystem.Application.Features.Common.Pagination;
using AuditSystem.Contract.Interfaces.ModelServices.AuditServices;
using AuditSystem.Contract.Models.Audit;
using AuditSystem.Contract.Models.Pagination;
using MediatR;

namespace AuditSystem.Application.Features.Audit.AuditUniverseObjective.Search;

internal sealed class SearchAuditUniverseObjectiveQueryHandler(IAuditUniverseObjectiveService auditUniverseObjectiveService) 
    : IRequestHandler<SearchAuditUniverseObjectiveQuery, Result<PaginationRequestResponse<AuditUniverseObjectiveModel>>>
{
    public async Task<Result<PaginationRequestResponse<AuditUniverseObjectiveModel>>> Handle(SearchAuditUniverseObjectiveQuery request, CancellationToken cancellationToken)
    {
        var page = new Page(request.PageNumber, request.PageSize);
        var searchResult = await auditUniverseObjectiveService.SearchAsync(request.SearchTerm, page);
        var response = new PaginationRequestResponse<AuditUniverseObjectiveModel>(searchResult.Items, searchResult.TotalCount);
        return Result.Success(response);
    }
}