using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.RiskControls.Objective.Delete;

public sealed record DeleteObjectiveCommand(
    Guid Id
) : UserAwareCommand<Result>;
