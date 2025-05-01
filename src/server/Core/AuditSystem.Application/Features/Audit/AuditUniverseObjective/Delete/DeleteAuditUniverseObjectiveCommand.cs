using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.Audit.AuditUniverseObjective.Delete;

public sealed class DeleteAuditUniverseObjectiveCommand : ICommand<Result>
{
    public required Guid Id { get; set; } = Guid.Empty;
}