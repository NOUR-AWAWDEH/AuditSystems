using AuditSystem.Application.Features.Common.Pagination;
using AuditSystem.Contract.Models.Audit;

namespace AuditSystem.Application.Features.Audit.BusinessObjective.Search;

public sealed record class SearchBusinessObjectiveQuery : PaginationRequest<BusinessObjectiveModel>
{
    public string? SearchTerm { get; set; } = string.Empty;
}