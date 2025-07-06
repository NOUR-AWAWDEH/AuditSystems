using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.Audit.BusinessObjective.Create;

public sealed record class CreateBusinessObjectiveCommand : ICommand<Result<CreateBusinessObjectiveCommandResponse>>
{
    public required string Impact { get; set; } = string.Empty;
    public required int Amount { get; set; }
}