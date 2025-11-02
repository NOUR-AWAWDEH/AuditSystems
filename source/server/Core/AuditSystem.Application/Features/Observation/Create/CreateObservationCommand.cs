using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.Observation.Create;

public sealed record CreateObservationCommand(
    string Description,
    string ActionOwner,
    string Background,
    string ActionPlan,
    DateTime ActionDate,
    string? Note,
    Guid AuditProgramId
) : UserAwareCommand<Result<CreateObservationCommandResponse>>;
