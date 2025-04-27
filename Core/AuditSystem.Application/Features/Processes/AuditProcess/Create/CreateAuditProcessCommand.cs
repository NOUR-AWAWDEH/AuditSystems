using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.Processes.AuditProcess.Create;

public sealed record class CreateAuditProcessCommand : ICommand<Result<CreateAuditProcessCommandResponse>>
{
    public required string ProcessName { get; set; } = string.Empty;
    public required Guid RatingId { get; set; }
    public string Description { get; set; } = string.Empty;
}
