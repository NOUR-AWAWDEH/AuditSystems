using AuditSystem.Application.Features.Common.Pagination;
using AuditSystem.Contract.Interfaces;
using AuditSystem.Contract.Models.Risks;

namespace AuditSystem.Application.Features.Risks.ControlDesign.Search;

public sealed record class SearchControlDesignQuery(
    string? SearchTerm,
    Guid ExitControlId
) : PaginationRequest<ControlDesignModel>, IUserAwareRequest
{ 
    public Guid UserId { get; set; } = Guid.Empty;
}