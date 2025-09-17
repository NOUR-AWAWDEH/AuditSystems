using AuditSystem.Application.Features.Common.Pagination;
using AuditSystem.Contract.Models.Organization;

namespace AuditSystem.Application.Features.Organization.Company.Search;

public class SearchCompanyQueryValidator
    : PaginationRequestValidator<CompanyModel, SearchCompanyQuery>;