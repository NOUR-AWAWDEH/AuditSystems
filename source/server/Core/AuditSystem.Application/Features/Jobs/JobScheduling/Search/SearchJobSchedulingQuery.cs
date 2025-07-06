using AuditSystem.Application.Features.Common.Pagination;
using AuditSystem.Contract.Models.Jobs;

namespace AuditSystem.Application.Features.Jobs.JobScheduling.Search;

public sealed record class SearchJobSchedulingQuery : PaginationRequest<JobSchedulingModel>
{
    public string? SearchTerm { get; set; } = string.Empty;
}