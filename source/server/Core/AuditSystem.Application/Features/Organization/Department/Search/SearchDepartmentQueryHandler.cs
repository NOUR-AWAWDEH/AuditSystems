using Ardalis.Result;
using AuditSystem.Application.Features.Common.Pagination;
using AuditSystem.Contract.Interfaces.ModelServices.OrganizationServices.DepartmentServices;

using AuditSystem.Contract.Models.Organization;
using AuditSystem.Contract.Models.Pagination;
using MediatR;

namespace AuditSystem.Application.Features.Organization.Department.Search;

internal sealed class SearchDepartmentQueryHandler(IDepartmentService departmentService)
    : IRequestHandler<SearchDepartmentQuery, Result<PaginationRequestResponse<DepartmentModel>>>
{
    public async Task<Result<PaginationRequestResponse<DepartmentModel>>> Handle(SearchDepartmentQuery request, CancellationToken cancellationToken)
    {
        var page = new Page(request.PageNumber, request.PageSize);
        var searchResult = await departmentService.SearchByUserIdAsync(request.UserId, request.SearchTerm, request.SubLocationId, page);
        var response = new PaginationRequestResponse<DepartmentModel>(searchResult.Items, searchResult.TotalCount);
        return Result.Success(response);
    }
}