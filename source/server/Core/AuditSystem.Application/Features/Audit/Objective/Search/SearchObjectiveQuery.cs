using AuditSystem.Application.Features.Common.Pagination;
using AuditSystem.Contract.Models.Audit;

namespace AuditSystem.Application.Features.Audit.Objective.Search;

public sealed record class SearchObjectiveQuery : PaginationRequest<ObjectiveModel>
{
    public string? SearchTerm { get; set; } = string.Empty;
}