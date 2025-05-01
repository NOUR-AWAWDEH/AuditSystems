using Ardalis.Result;
using AuditSystem.Application.Features.Common.Pagination;
using AuditSystem.Contract.Interfaces.ModelServices.OrganizationServices;
using AuditSystem.Contract.Models.Organization;
using AuditSystem.Contract.Models.Pagination;
using MediatR;

namespace AuditSystem.Application.Features.Organization.SubDepartment.Search;

internal sealed class SearchSubDepartmentQueryHandler(ISubDepartmentService subDepartmentService) :
    IRequestHandler<SearchSubDepartmentQuery, Result<PaginationRequestResponse<SubDepartmentModel>>>
{
    public async Task<Result<PaginationRequestResponse<SubDepartmentModel>>> Handle(SearchSubDepartmentQuery request, CancellationToken cancellationToken)
    {
        var page = new Page(request.PageNumber, request.PageSize);
        var searchResult = await subDepartmentService.SearchAsync(request.SearchTerm, page);
        var response = new PaginationRequestResponse<SubDepartmentModel>(searchResult.Items, searchResult.TotalCount);
        return Result.Success(response);
    }
}