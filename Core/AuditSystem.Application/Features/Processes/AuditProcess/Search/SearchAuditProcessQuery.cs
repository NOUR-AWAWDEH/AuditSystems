using AuditSystem.Application.Features.Common.Pagination;
using AuditSystem.Contract.Models.Processes;

namespace AuditSystem.Application.Features.Processes.AuditProcess.Search;

public sealed record class SearchAuditProcessQuery : PaginationRequest<AuditProcessModel>
{
    public string? SearchTerm { get; set; } = string.Empty;
}