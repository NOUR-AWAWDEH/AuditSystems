using AuditSystem.Application.Features.Common.Pagination;
using AuditSystem.Contract.Models.RiskControls;

namespace AuditSystem.Application.Features.RiskControls.RiskProgram.Search;

public sealed record class SearchRiskProgramQuery : PaginationRequest<RiskProgramModel>
{
    public string? SearchTerm { get; set; } = string.Empty;
}