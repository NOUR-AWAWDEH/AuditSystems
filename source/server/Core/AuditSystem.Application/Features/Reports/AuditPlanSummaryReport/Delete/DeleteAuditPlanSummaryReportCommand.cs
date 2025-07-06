using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.Reports.AuditPlanSummaryReport.Delete;

public sealed class DeleteAuditPlanSummaryReportCommand : ICommand<Result>
{
    public required Guid Id { get; set; } = Guid.Empty;
}