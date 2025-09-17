using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.RiskControls.RiskControlMatrix.Delete;

public sealed record DeleteRiskControlMatrixCommand(
    Guid Id
) : UserAwareCommand<Result>;