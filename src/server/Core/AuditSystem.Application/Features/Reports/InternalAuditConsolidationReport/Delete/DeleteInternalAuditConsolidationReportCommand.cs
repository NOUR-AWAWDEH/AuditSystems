using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.Reports.InternalAuditConsolidationReport.Delete;

public sealed class DeleteInternalAuditConsolidationReportCommand : ICommand<Result>
{
    public required Guid Id { get; set; } = Guid.Empty;
}