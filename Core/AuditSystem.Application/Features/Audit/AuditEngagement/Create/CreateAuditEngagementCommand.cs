using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.Audit.AuditEngagement.Create;

public sealed record class CreateAuditEngagementCommand : ICommand<Result<CreateAuditEngagementCommandResponse>>
{
    public required string JobName { get; set; } = string.Empty;
    public DateOnly PlannedStartDate { get; set; }
    public DateOnly PlannedEndDate { get; set; }
    public string JobType { get; set; } = string.Empty;
    public required Guid LocationId { get; set; }
    public string Status { get; set; } = string.Empty;
    public string JobStatus { get; set; } = string.Empty;
}