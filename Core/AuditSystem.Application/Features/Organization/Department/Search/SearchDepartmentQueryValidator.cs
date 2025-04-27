using AuditSystem.Application.Features.Common.Pagination;
using AuditSystem.Contract.Models.Organization;

namespace AuditSystem.Application.Features.Organization.Department.Search;

public class SearchDepartmentQueryValidator : PaginationRequestValidator<DepartmentModel, SearchDepartmentQuery>;