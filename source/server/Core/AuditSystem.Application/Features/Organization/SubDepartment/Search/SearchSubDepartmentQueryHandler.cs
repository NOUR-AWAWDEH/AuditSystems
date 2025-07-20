using Ardalis.Result;
using AuditSystem.Application.Features.Common.Pagination;
using AuditSystem.Contract.Interfaces.ModelServices.OrganizationServices.SubDepartmentServices;
using AuditSystem.Contract.Models.Organization;
using AuditSystem.Contract.Models.Pagination;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AuditSystem.Application.Features.Organization.SubDepartment.Search;

internal sealed class SearchSubDepartmentQueryHandler(
    ISubDepartmentService subDepartmentService,
    ILogger<SearchSubDepartmentQueryHandler> logger) 
    : IRequestHandler<SearchSubDepartmentQuery, Result<PaginationRequestResponse<SubDepartmentModel>>>
{
    public async Task<Result<PaginationRequestResponse<SubDepartmentModel>>> Handle(SearchSubDepartmentQuery request, CancellationToken cancellationToken)
    {
        var page = new Page(request.PageNumber, request.PageSize);
        var searchResult = await subDepartmentService.SearchByUserIdAsync(request.UserId, request.SearchTerm, request.DepartmentId, page);
        
        logger.LogInformation("Sub-Departments retrieved successfully for user ID: {UserId}", request.UserId);
        var response = new PaginationRequestResponse<SubDepartmentModel>(searchResult.Items, searchResult.TotalCount);
        return Result.Success(response);
    }
}