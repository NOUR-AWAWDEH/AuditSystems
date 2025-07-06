using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.Reports.ReportingFollowUp.Create;

public sealed record class CreateReportingFollowUpCommand : ICommand<Result<CreateReportingFollowUpCommandResponse>>
{
    public required string Reporting { get; set; } = string.Empty;
    public required string FollowUp { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
}
