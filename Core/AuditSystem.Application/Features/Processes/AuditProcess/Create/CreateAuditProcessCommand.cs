using Ardalis.Result;
using AuditSystem.Application.Base;
using AuditSystem.Application.Features.Processes.AuditProcess.Creat;

namespace AuditSystem.Application.Features.Processes.AuditProcess.Create;

public sealed record class CreateAuditProcessCommand : ICommand<Result<CreateAuditProcessCommandResponse>>
{
    public required Guid AuditSettingsId { get; set; }
    public string ProcessName { get; set; } = string.Empty;
    public Guid RatingId { get; set; }
    public string Description { get; set; } = string.Empty;
}
