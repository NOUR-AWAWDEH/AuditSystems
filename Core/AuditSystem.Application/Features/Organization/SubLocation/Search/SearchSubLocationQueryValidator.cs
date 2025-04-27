using AuditSystem.Application.Features.Common.Pagination;
using AuditSystem.Contract.Models.Organization;

namespace AuditSystem.Application.Features.Organization.SubLocation.Search;

public class SearchSubLocationQueryValidator
    : PaginationRequestValidator<SubLocationModel, SearchSubLocationQuery>;