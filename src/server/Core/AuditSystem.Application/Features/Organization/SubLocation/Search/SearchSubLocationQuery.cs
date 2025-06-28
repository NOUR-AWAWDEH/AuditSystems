using AuditSystem.Application.Features.Common.Pagination;
using AuditSystem.Contract.Models.Organization;

namespace AuditSystem.Application.Features.Organization.SubLocation.Search;

public sealed record SearchSubLocationQuery(
    string? SearchTerm,
    Guid UserId
) : PaginationRequest<SubLocationModel>;