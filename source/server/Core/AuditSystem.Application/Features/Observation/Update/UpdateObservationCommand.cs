using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.Observation.Update;

public sealed record UpdateObservationCommand(
    Guid Id,
    string Highlight,
    string Background,
    bool IsActive,
    string? Note,
    Guid AuditProgramId,
    Guid RateId
) : UserAwareCommand<Result>;