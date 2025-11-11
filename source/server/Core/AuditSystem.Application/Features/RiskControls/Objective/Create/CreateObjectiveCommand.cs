using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.RiskControls.Objective.Create;


public sealed record class CreateObjectiveCommand(
    string Description,
    Guid RiskControlMatrixId
) : UserAwareCommand<Result<CreateObjectiveCommandResponse>>;
