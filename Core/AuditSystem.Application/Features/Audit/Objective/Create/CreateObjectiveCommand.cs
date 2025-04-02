using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.Audit.Objective.Create;

public sealed record class CreateObjectiveCommand : ICommand<Result<CreateObjectiveCommandResponse>>
{
    public required Guid RiskControlMatrixId { get; set; }
    public Guid RatingId { get; set; }
    public string Description { get; set; } = string.Empty;
}
