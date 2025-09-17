using AuditSystem.Application.Features.Common.Pagination;
using AuditSystem.Contract.Models.Organization;
using AuditSystem.Contract.Interfaces;

namespace AuditSystem.Application.Features.Organization.Location.Search;

public sealed record SearchLocationQuery(
    string? SearchTerm
) : PaginationRequest<LocationModel>, IUserAwareRequest
{
    public Guid UserId { get; set; }
}