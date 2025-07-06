using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.Audit.BusinessObjective.Update;

public sealed record class UpdateBusinessObjectiveCommand : ICommand<Result>
{
    public required Guid Id { get; init; } = Guid.Empty;
    public required string Impact { get; set; } = string.Empty;
    public required int Amount { get; set; }
}