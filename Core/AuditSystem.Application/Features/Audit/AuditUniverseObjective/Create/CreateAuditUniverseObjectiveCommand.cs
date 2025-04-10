using Ardalis.Result;
using AuditSystem.Application.Base;
namespace AuditSystem.Application.Features.Audit.AuditUniverseObjective.Create;

public sealed record class CreateAuditUniverseObjectiveCommand : ICommand<Result<CreateAuditUniverseObjectiveCommandResponse>>
{
    public required Guid AuditUniverseID { get; set; }
    public required string Impact { get; set; } = string.Empty;
    public int Amount { get; set; }
    public int ImpactAmount { get; set; }
    public double Percentage { get; set; }
}