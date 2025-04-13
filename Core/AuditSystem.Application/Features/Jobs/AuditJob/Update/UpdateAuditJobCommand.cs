using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.Jobs.AuditJob.Update;

public sealed record class UpdateAuditJobCommand : ICommand<Result>
{
    public required Guid Id { get; init; } = Guid.Empty;
    public required string JobName { get; set; } = string.Empty;
    public required string JobType { get; set; } = string.Empty;
    public required Guid AuditUniverseID { get; set; }
}
