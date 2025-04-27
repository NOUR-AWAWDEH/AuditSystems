using AuditSystem.Application.Features.Common.Pagination;
using AuditSystem.Contract.Models.Organization;

namespace AuditSystem.Application.Features.Organization.Location.Search;

public sealed record class SearchLocationQuery : PaginationRequest<LocationModel>
{
    public string? SearchTerm { get; set; } = string.Empty;
}
