using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.Reports.JobTimeAllocationReport.Create;

public sealed record class CreateJobTimeAllocationReportCommand : ICommand<Result<CreateJobTimeAllocationReportCommandResponse>>
{
    public required string JobName { get; set; } = string.Empty;
    public required string ReportName { get; set; } = string.Empty;
    public required DateOnly ReportDate { get; set; }
    public required Guid CreatedById { get; set; }
    public string Status { get; set; } = string.Empty;
}
