using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.RiskControls.RiskControlMatrix.Create;

public sealed record class CreateRiskControlMatrixCommand(
    string Description,
    Guid AuditProcessId
) : UserAwareCommand<Result<CreateRiskControlMatrixCommandResponse>>;
