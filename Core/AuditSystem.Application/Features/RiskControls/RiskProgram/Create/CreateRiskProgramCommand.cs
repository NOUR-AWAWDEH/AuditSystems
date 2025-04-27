using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.RiskControls.RiskProgram.Create;

public sealed record class CreateRiskProgramCommand : ICommand<Result<CreateRiskProgramCommandResponse>>
{
    public required Guid RiskControlId { get; set; }
    public required string Name { get; set; } = string.Empty;
    public required Guid RatingId { get; set; }
    public string Description { get; set; } = string.Empty;
}
