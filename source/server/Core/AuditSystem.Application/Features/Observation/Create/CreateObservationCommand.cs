using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.Observation.Create;

public sealed record CreateObservationCommand(
    string Highlight,
    string Background,
    bool IsActive,
    string? Note,
    Guid AuditProgramId,
    Guid RateId
) : UserAwareCommand<Result<CreateObservationCommandResponse>>;
