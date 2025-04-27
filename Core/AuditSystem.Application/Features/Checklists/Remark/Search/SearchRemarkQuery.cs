using AuditSystem.Application.Features.Common.Pagination;
using AuditSystem.Contract.Models.Checklists;

namespace AuditSystem.Application.Features.Checklists.Remark.Search;

public sealed record class SearchRemarkQuery : PaginationRequest<RemarkModel>
{
    public string? SearchTearm { get; set; } = string.Empty;
}