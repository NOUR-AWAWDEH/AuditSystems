using AuditSystem.Application.Features.Common.Pagination;
using AuditSystem.Contract.Models.Audit;

namespace AuditSystem.Application.Features.Audit.AuditUniverse.Search;

public sealed record class SearchAuditUniverseQuery : PaginationRequest<AuditUniverseModel>
{
    public string? SearchTearm { get; set; } = string.Empty;
}