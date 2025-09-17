using AuditSystem.Application.Features.Common.Pagination;
using AuditSystem.Contract.Interfaces;
using AuditSystem.Contract.Models.RiskControls;

namespace AuditSystem.Application.Features.RiskControls.Objective.Search;

public sealed record class SearchObjectiveQuery(
    string? SearchTerm,
    Guid RiskControlMatrixId
) : PaginationRequest<ObjectiveModel>, IUserAwareRequest
{
    public Guid UserId { get; set; }
}

