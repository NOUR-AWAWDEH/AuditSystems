using AuditSystem.Application.Features.Common.Pagination;
using AuditSystem.Contract.Models.Organization;

namespace AuditSystem.Application.Features.Organization.SubDepartment.Search;

public class SearchSubDepartmentQueryValidator
    : PaginationRequestValidator<SubDepartmentModel, SearchSubDepartmentQuery>;