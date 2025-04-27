using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.Jobs.AuditJob.Create;

public sealed record class CreateAuditJobCommand : ICommand<Result<CreateAuditJobCommandResponse>>
{
    public required string JobName { get; set; } = string.Empty;
    public required string JobType { get; set; } = string.Empty;
    public required Guid AuditUniverseID { get; set; }
}
