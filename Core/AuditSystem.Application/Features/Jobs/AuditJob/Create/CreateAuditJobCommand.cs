using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.Jobs.AuditJob.Create;

public sealed record class CreateAuditJobCommand : ICommand<Result<CreateAuditJobCommandResponse>>
{
    public Guid AuditUniverseID { get; set; }
    public string JobName { get; set; } = string.Empty;
    public string JobType { get; set; } = string.Empty;
}
