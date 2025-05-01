using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.Reports.AuditPlanSummaryReport.Update;

public sealed record class UpdateAuditPlanSummaryReportCommand : ICommand<Result>
{
    public required Guid Id { get; init; } = Guid.Empty;
    public required string ReportName { get; set; } = string.Empty;
    public required DateOnly ReportDate { get; set; }
    public required Guid CreatedById { get; set; }
    public string Status { get; set; } = string.Empty;
}
