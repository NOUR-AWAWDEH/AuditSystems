using Ardalis.Result;
using AuditSystem.Application.Features.Common.Pagination;
using AuditSystem.Contract.Interfaces.ModelServices.OrganizationServices;
using AuditSystem.Contract.Models.Organization;
using AuditSystem.Contract.Models.Pagination;
using MediatR;

namespace AuditSystem.Application.Features.Organization.Company.Search;

internal sealed class SearchCompanyQueryHandler(ICompanyService companyService)
    : IRequestHandler<SearchCompanyQuery, Result<PaginationRequestResponse<CompanyModel>>>
{
    public async Task<Result<PaginationRequestResponse<CompanyModel>>> Handle(SearchCompanyQuery request, CancellationToken cancellationToken)
    {
        var page = new Page(request.PageNumber, request.PageSize);
        var searchResult = await companyService.SearchByUserIdAsync(request.UserId, request.SearchTerm, page);
        var response = new PaginationRequestResponse<CompanyModel>(searchResult.Items, searchResult.TotalCount);
        return Result.Success(response);
    }
}