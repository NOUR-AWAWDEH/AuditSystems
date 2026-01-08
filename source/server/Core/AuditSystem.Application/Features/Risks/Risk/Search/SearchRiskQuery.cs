using AuditSystem.Application.Features.Common.Pagination;
using AuditSystem.Contract.Interfaces;
using AuditSystem.Contract.Models.Risks;

namespace AuditSystem.Application.Features.Risks.Risk.Search;

public sealed record class SearchRiskQuery(
    string? SearchTerm,
    Guid ObservationId
) : PaginationRequest<RiskModel>, IUserAwareRequest
{ 
    public Guid UserId { get; set; } = Guid.Empty;
}
