using AuditSystem.Application.Features.Common.Pagination;
using AuditSystem.Contract.Models.Tasks;

namespace AuditSystem.Application.Features.Tasks.Search;

public sealed record class SearchTaskManagementQuery : PaginationRequest<TaskManagementModel>
{
    public string? SearchTerm { get; set; } = string.Empty;
}