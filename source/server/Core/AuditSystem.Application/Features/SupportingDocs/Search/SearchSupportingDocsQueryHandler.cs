using Ardalis.Result;
using AuditSystem.Application.Features.Common.Pagination;
using AuditSystem.Contract.Interfaces.ModelServices.SupportingDocsServices;
using AuditSystem.Contract.Models.Pagination;
using AuditSystem.Contract.Models.SupportingDocs;
using MediatR;

namespace AuditSystem.Application.Features.SupportingDocs.Search;

internal sealed class SearchSupportingDocsQueryHandler(ISupportingDocService supportingDocService)
     : IRequestHandler<SearchSupportingDocsQuery, Result<PaginationRequestResponse<SupportingDocModel>>>
{
    public async Task<Result<PaginationRequestResponse<SupportingDocModel>>> Handle(SearchSupportingDocsQuery request, CancellationToken cancellationToken)
    {
       var page = new Page(request.PageNumber, request.PageSize);
       var searchResult = await supportingDocService.SearchByUserIdAsync(request.UserId, request.SearchTerm, page);
       var response = new PaginationRequestResponse<SupportingDocModel>(searchResult.Items, searchResult.TotalCount);
       return Result.Success(response);
    }
}
