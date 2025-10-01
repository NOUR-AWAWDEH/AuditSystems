using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.Observation.Update;

public sealed record UpdateObservationCommand(
    Guid Id,
    string Description,
    string? Recommendation,
    string ActionPlan,
    DateTime ActionDate,
    Guid AuditProgramId
) : UserAwareCommand<Result>;