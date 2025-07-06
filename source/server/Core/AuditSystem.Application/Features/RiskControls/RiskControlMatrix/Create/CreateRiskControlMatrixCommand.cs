using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.RiskControls.RiskControlMatrix.Create;

public sealed record class CreateRiskControlMatrixCommand : ICommand<Result<CreateRiskControlMatrixCommandResponse>>
{
    public required Guid SubProcessId { get; set; }
    public string Description { get; set; } = string.Empty;
}
