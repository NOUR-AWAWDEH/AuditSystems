using AuditSystem.Application.Features.Common.Pagination;
using AuditSystem.Contract.Models.Organization;

namespace AuditSystem.Application.Features.Organization.SubLocation.Search;

public sealed record class SearchSubLocationQuery : PaginationRequest<SubLocationModel>
{
    public string? SearchTerm { get; set; } = string.Empty;
    public Guid UserId { get; set; } = Guid.Empty;
}