using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.Observation.Update;

public sealed record UpdateObservationCommand(
    Guid Id,
    string Description,
    string ActionOwner,
    string Background,
    string ActionPlan,
    DateTime ActionDate,
    string? Note,
    Guid AuditProgramId
) : UserAwareCommand<Result>;