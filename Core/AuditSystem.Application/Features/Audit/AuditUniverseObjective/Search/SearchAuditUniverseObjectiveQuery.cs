using AuditSystem.Application.Features.Common.Pagination;
using AuditSystem.Contract.Models.Audit;

namespace AuditSystem.Application.Features.Audit.AuditUniverseObjective.Search;

public sealed record class SearchAuditUniverseObjectiveQuery : PaginationRequest<AuditUniverseObjectiveModel>
{
    public string? SearchTearm { get; set; } = string.Empty;
}