using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.Reports.InternalAuditConsolidationReport.Create;

public sealed record class CreateInternalAuditConsolidationReportCommand : ICommand<Result<CreateInternalAuditConsolidationReportCommandResponse>>
{
    public required string JobName { get; set; } = string.Empty;
    public required string ReportName { get; set; } = string.Empty;
    public required DateOnly ReportDate { get; set; }
    public required Guid PreparedByUserId { get; set; }
    public string Status { get; set; } = string.Empty;
}
