using AuditSystem.Application.Features.Common.Pagination;
using AuditSystem.Contract.Interfaces;
using AuditSystem.Contract.Models.Audit;

namespace AuditSystem.Application.Features.Audit.AuditProgram.Search;

public sealed record SearchAuditProgramQuery(
    string? SearchTerm,
    Guid ExitControlId
) : PaginationRequest<AuditProgramModel>, IUserAwareRequest
{
    public Guid UserId { get; set; }
}