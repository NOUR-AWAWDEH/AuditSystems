using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.Reports.AuditExceptionReport.Delete;

public sealed class DeleteAuditExceptionReportCommand : ICommand<Result>
{
    public required Guid Id { get; set; } = Guid.Empty;
}
