using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.RiskControls.Objective.Update;

public sealed record class UpdateObjectiveCommand(
    Guid Id,
    Guid RiskControlMatrixId,
    string Description
) : UserAwareCommand<Result>;
