using Ardalis.Result;
using AuditSystem.Application.Features.Common.Pagination;
using AuditSystem.Contract.Interfaces.ModelServices.AuditServices;
using AuditSystem.Contract.Models.Audit;
using AuditSystem.Contract.Models.Pagination;
using MediatR;

namespace AuditSystem.Application.Features.Audit.SpecialProject.Search;

internal sealed class SearchSpecialProjectQueryHandler(ISpecialProjectService specialProjectService)
    : IRequestHandler<SearchSpecialProjectQuery, Result<PaginationRequestResponse<SpecialProjectModel>>>
{
    public async Task<Result<PaginationRequestResponse<SpecialProjectModel>>> Handle(SearchSpecialProjectQuery request, CancellationToken cancellationToken)
    {
        var page = new Page(request.PageNumber, request.PageSize);
        var searchResult = await specialProjectService.SearchAsync(request.SearchTerm, page);
        var response = new PaginationRequestResponse<SpecialProjectModel>(searchResult.Items, searchResult.TotalCount);
        return Result.Success(response);
    }
}