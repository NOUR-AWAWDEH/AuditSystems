using AuditSystem.Application.Features.Common.Pagination;
using AuditSystem.Contract.Interfaces;
using AuditSystem.Contract.Models.Risks;

namespace AuditSystem.Application.Features.Risks.ExitControl.Search;

public sealed record class SearchExitControlQuery(
    string? SearchTerm,
    Guid RiskId
) : PaginationRequest<ExitControlModel>, IUserAwareRequest
{ 
    public Guid UserId { get; set; } = Guid.Empty;
}
