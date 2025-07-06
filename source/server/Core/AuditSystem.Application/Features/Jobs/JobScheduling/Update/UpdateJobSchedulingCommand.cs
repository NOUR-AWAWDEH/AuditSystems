using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.Jobs.JobScheduling.Update;

public sealed record class UpdateJobSchedulingCommand : ICommand<Result>
{
    public required Guid Id { get; init; } = Guid.Empty;
    public required string AuditableUnit { get; set; } = string.Empty;
    public required DateOnly AuditYear { get; set; }
    public required DateOnly PlannedStartDate { get; set; }
    public DateOnly PlannedEndDate { get; set; }
    public string Status { get; set; } = string.Empty;
}
