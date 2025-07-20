using AuditSystem.Application.Features.Common.Pagination;
using AuditSystem.Contract.Interfaces;
using AuditSystem.Contract.Models.Processes;

namespace AuditSystem.Application.Features.Processes.AuditProcess.Search;

public sealed record class SearchAuditProcessQuery(
    string? SearchTerm,
    Guid SubDepartmentId
) : PaginationRequest<AuditProcessModel>, IUserAwareRequest
{
    public Guid UserId { get; set; } = Guid.Empty;
}