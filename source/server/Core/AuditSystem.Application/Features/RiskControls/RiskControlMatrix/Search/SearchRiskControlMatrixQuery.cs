using AuditSystem.Application.Features.Common.Pagination;
using AuditSystem.Contract.Interfaces;
using AuditSystem.Contract.Models.RiskControls;

namespace AuditSystem.Application.Features.RiskControls.RiskControlMatrix.Search;

public sealed record SearchRiskControlMatrixQuery(
    string? SearchTerm,
    Guid AuditProcessId
) : PaginationRequest<RiskControlMatrixModel>, IUserAwareRequest
{
    public Guid UserId { get; set; }
}