using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.RiskControls.RiskControlMatrix.Update;

public sealed record class UpdateRiskControlMatrixCommand : ICommand<Result>
{
    public required Guid Id { get; init; } = Guid.Empty;
    public required Guid SubProcessId { get; set; }
    public string Description { get; set; } = string.Empty;
}
