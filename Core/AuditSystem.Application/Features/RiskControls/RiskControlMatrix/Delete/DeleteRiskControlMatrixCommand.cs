using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.RiskControls.RiskControlMatrix.Delete;

public sealed class DeleteRiskControlMatrixCommand : ICommand<Result>
{
    public required Guid Id { get; set; } = Guid.Empty;
}
