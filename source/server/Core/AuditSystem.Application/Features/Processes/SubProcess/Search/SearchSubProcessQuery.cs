using AuditSystem.Application.Features.Common.Pagination;
using AuditSystem.Contract.Models.Processes;

namespace AuditSystem.Application.Features.Processes.SubProcess.Search;

public sealed record class SearchSubProcessQuery : PaginationRequest<SubProcessModel>
{
    public string? SearchTerm { get; set; } = string.Empty;
}