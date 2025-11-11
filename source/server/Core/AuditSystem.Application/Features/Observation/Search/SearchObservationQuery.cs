using AuditSystem.Application.Features.Common.Pagination;
using AuditSystem.Contract.Interfaces;
using AuditSystem.Contract.Models.Observation;

namespace AuditSystem.Application.Features.Observation.Search;

public sealed record SearchObservationQuery(
    string? SearchTerm,
    Guid AuditProgramId
) : PaginationRequest<ObservationModel>, IUserAwareRequest
{
    public Guid UserId { get; set; }
}