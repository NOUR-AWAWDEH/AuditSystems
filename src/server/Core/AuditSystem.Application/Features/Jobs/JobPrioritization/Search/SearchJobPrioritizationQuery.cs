using AuditSystem.Application.Features.Common.Pagination;
using AuditSystem.Contract.Models.Jobs;

namespace AuditSystem.Application.Features.Jobs.JobPrioritization.Search;

public sealed record class SearchJobPrioritizationQuery : PaginationRequest<JobPrioritizationModel>
{
    public string? SearchTerm { get; set; } = string.Empty;
}