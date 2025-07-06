using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.Reports.InternalAuditConsolidationReport.Update;

public sealed record class UpdateInternalAuditConsolidationReportCommand : ICommand<Result>
{
    public required Guid Id { get; init; } = Guid.Empty;
    public required string JobName { get; set; } = string.Empty;
    public required string ReportName { get; set; } = string.Empty;
    public required DateOnly ReportDate { get; set; }
    public required Guid PreparedByUserId { get; set; }
    public string Status { get; set; } = string.Empty;
}