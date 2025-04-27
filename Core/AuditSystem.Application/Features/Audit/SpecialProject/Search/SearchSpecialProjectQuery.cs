using AuditSystem.Application.Features.Common.Pagination;
using AuditSystem.Contract.Models.Audit;

namespace AuditSystem.Application.Features.Audit.SpecialProject.Search;

public sealed record class SearchSpecialProjectQuery : PaginationRequest<SpecialProjectModel>
{
    public string? SearchTearm { get; set; } = string.Empty;
}