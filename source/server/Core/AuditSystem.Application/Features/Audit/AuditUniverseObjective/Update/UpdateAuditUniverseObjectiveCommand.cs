using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.Audit.AuditUniverseObjective.Update;

public sealed record class UpdateAuditUniverseObjectiveCommand : ICommand<Result>
{
    public required Guid Id { get; init; } = Guid.Empty;
    public required Guid AuditUniverseID { get; set; }
    public required string Impact { get; set; } = string.Empty;
    public int Amount { get; set; }
    public int ImpactAmount { get; set; }
    public double Percentage { get; set; }
}