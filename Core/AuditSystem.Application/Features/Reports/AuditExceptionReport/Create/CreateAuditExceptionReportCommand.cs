using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.Reports.AuditExceptionReport.Create;

public sealed record class CreateAuditExceptionReportCommand : ICommand<Result<CreateAuditExceptionReportCommandResponse>>
{
    public required string ReportName { get; set; } = string.Empty;
    public required DateOnly ReportDate { get; set; }
    public required Guid CreatedById { get; set; }
    public string Status { get; set; } = string.Empty;
}
