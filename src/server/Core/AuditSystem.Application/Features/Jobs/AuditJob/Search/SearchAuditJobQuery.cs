using AuditSystem.Application.Features.Common.Pagination;
using AuditSystem.Contract.Models.Jobs;

namespace AuditSystem.Application.Features.Jobs.AuditJob.Search;

public sealed record class SearchAuditJobQuery : PaginationRequest<AuditJobModel>
{
    public string? SearchTerm { get; set; } = string.Empty;
}