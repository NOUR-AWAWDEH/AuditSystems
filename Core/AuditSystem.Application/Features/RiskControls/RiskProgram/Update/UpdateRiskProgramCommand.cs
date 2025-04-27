using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.RiskControls.RiskProgram.Update;

public sealed record class UpdateRiskProgramCommand : ICommand<Result>
{
    public required Guid Id { get; init; } = Guid.Empty;
    public required Guid RiskControlId { get; set; }
    public required string Name { get; set; } = string.Empty;
    public required Guid RatingId { get; set; }
    public string Description { get; set; } = string.Empty;
}
