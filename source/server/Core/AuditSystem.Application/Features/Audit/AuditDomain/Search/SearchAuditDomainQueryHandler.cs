using Ardalis.Result;
using AuditSystem.Application.Features.Common.Pagination;
using AuditSystem.Contract.Interfaces.ModelServices.AuditServices;
using AuditSystem.Contract.Models.Audit;
using AuditSystem.Contract.Models.Pagination;
using MediatR;

namespace AuditSystem.Application.Features.Audit.AuditDomain.Search;

internal sealed class SearchAuditDomainQueryHandler(IAuditDomainService auditDomainService) 
    : IRequestHandler<SearchAuditDomainQuery, Result<PaginationRequestResponse<AuditDomainModel>>>
{
    public async Task<Result<PaginationRequestResponse<AuditDomainModel>>> Handle(SearchAuditDomainQuery request, CancellationToken cancellationToken) 
    {
        var page = new Page(request.PageNumber, request.PageSize);
        var searchResult = await auditDomainService.SearchAsync(request.SearchTerm, page);
        var response = new PaginationRequestResponse<AuditDomainModel>(searchResult.Items, searchResult.TotalCount);
        return Result.Success(response);
    }
}