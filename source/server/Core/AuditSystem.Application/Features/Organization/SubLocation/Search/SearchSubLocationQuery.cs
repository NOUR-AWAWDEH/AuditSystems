using AuditSystem.Application.Features.Common.Pagination;
using AuditSystem.Contract.Interfaces;
using AuditSystem.Contract.Models.Organization;

namespace AuditSystem.Application.Features.Organization.SubLocation.Search;

public sealed record SearchSubLocationQuery(
    string? SearchTerm
) : PaginationRequest<SubLocationModel>, IUserAwareRequest
{
    public Guid UserId { get; set; }
}