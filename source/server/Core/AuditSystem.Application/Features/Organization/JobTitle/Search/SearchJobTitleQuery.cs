using AuditSystem.Application.Features.Common.Pagination;
using AuditSystem.Contract.Interfaces;
using AuditSystem.Contract.Models.Organization;

namespace AuditSystem.Application.Features.Organization.JobTitle.Search;

public sealed record SearchJobTitleQuery(
    string? SearchTerm,
    Guid CompanyId
) : PaginationRequest<JobTitleModel>, IUserAwareRequest
{
    public Guid UserId { get; set; }
}
