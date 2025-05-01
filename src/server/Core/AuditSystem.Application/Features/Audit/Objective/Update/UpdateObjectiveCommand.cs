using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.Audit.Objective.Update;

public sealed record class UpdateObjectiveCommand : ICommand<Result>
{
    public required Guid Id { get; init; } = Guid.Empty;
    public required Guid RiskControlMatrixId { get; set; }
    public required Guid RatingId { get; set; }
    public string Description { get; set; } = string.Empty;
}