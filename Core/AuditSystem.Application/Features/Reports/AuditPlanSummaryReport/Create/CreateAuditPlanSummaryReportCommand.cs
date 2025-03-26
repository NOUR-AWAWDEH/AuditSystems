using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.Reports.AuditPlanSummaryReport.Create;

public sealed record class CreateAuditPlanSummaryReportCommand : ICommand<Result<CreateAuditPlanSummaryReportCommandResponse>>
{
    public string ReportName { get; set; } = string.Empty;
    public DateOnly ReportDate { get; set; }
    public required Guid CreatedById { get; set; }
    public string Status { get; set; } = string.Empty;
}