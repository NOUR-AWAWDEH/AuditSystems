using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.Observation.Create;

public sealed record CreateObservationCommand(
    string Description,
    string? Recommendation,
    string ActionPlan,
    DateTime ActionDate,
    Guid AuditProgramId
) : UserAwareCommand<Result<CreateObservationCommandResponse>>;
