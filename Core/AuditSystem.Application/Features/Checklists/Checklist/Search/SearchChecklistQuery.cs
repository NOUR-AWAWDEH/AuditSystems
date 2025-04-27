using AuditSystem.Application.Features.Common.Pagination;
using AuditSystem.Contract.Models.Checklists;

namespace AuditSystem.Application.Features.Checklists.Checklist.Search;

public sealed record class SearchChecklistQuery : PaginationRequest<ChecklistModel>
{
    public string? SearchTerm { get; set; } = string.Empty;
}