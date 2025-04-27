using Ardalis.Result;
using AuditSystem.Application.Features.Common.Pagination;
using AuditSystem.Contract.Interfaces.ModelServices.AuditServices;
using AuditSystem.Contract.Models.Audit;
using AuditSystem.Contract.Models.Pagination;
using MediatR;

namespace AuditSystem.Application.Features.Audit.AuditUniverse.Search;

internal sealed class SearchAuditUniverseQueryHandler(IAuditUniverseService auditUniverseService) 
    : IRequestHandler<SearchAuditUniverseQuery,Result<PaginationRequestResponse<AuditUniverseModel>>>
{
    public async Task<Result<PaginationRequestResponse<AuditUniverseModel>>> Handle(SearchAuditUniverseQuery request, CancellationToken cancellationToken)
    {
       var page = new Page(request.PageNumber,request.PageSize);
       var searchResult = await auditUniverseService.SearchAsync(request.SearchTerm,page);
       var response = new PaginationRequestResponse<AuditUniverseModel>(searchResult.Items, searchResult.TotalCount);
       return Result.Success(response);
    }
}