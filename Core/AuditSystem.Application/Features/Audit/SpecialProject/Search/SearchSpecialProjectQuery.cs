using AuditSystem.Application.Features.Common.Pagination;
using AuditSystem.Contract.Models.Audit;

namespace AuditSystem.Application.Features.Audit.SpecialProject.Search;

public sealed record class SearchSpecialProjectQuery : PaginationRequest<SpecialProjectModel>
{
    public string? SearchTerm { get; set; } = string.Empty;
}