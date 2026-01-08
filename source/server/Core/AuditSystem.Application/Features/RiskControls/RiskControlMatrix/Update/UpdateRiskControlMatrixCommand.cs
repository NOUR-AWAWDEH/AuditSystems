using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.RiskControls.RiskControlMatrix.Update;

public sealed record class UpdateRiskControlMatrixCommand(
    Guid Id,
    Guid AuditProcessId,
    string Description
) : UserAwareCommand<Result>;