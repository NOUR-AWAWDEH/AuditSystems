using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.Observation.Delete;

public sealed record DeleteObservationCommand(
    Guid Id
) : UserAwareCommand<Result>;