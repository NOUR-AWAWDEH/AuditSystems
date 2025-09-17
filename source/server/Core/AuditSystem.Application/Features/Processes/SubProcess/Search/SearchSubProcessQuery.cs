using AuditSystem.Application.Features.Common.Pagination;
using AuditSystem.Contract.Interfaces;
using AuditSystem.Contract.Models.Processes;

namespace AuditSystem.Application.Features.Processes.SubProcess.Search;

public sealed record SearchSubProcessQuery(
    string? SearchTerm,
    Guid AuditProcessId
) : PaginationRequest<SubProcessModel>, IUserAwareRequest
{
    public Guid UserId { get; set; } = Guid.Empty;
}