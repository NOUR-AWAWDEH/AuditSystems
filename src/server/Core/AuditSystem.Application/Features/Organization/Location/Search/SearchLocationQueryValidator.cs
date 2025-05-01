using AuditSystem.Application.Features.Common.Pagination;
using AuditSystem.Contract.Models.Organization;

namespace AuditSystem.Application.Features.Organization.Location.Search;

public class SearchLocationQueryValidator
    : PaginationRequestValidator<LocationModel, SearchLocationQuery>;