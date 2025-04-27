using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.Jobs.JobScheduling.Create;

public sealed record class CreateJobSchedulingCommand : ICommand<Result<CreateJobSchedulingCommandResponse>>
{
    public required string AuditableUnit { get; set; } = string.Empty;
    public required DateOnly AuditYear { get; set; }
    public required DateOnly PlannedStartDate { get; set; }
    public DateOnly PlannedEndDate { get; set; }
    public string Status { get; set; } = string.Empty;
}
