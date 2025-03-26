using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.Jobs.JobScheduling.Create;

public sealed record class CreateJobSchedulingCommand : ICommand<Result<CreateJobSchedulingCommandResponse>>
{
    public string AuditableUnit { get; set; } = string.Empty;
    public DateOnly AuditYear { get; set; }
    public DateOnly PlannedStartDate { get; set; }
    public DateOnly PlannedEndDate { get; set; }
    public string Status { get; set; } = string.Empty;
}
