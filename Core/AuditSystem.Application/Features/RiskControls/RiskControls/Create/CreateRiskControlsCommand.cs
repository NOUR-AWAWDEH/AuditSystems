using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.RiskControls.RiskControls.Create;

public sealed record class CreateRiskControlsCommand : ICommand<Result<CreateRiskControlsCommandResponse>>
{
    public required Guid RiskId { get; set; }
    public Guid RatingId { get; set; }
    public string Description { get; set; } = string.Empty;
}
