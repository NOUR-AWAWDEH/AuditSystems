using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.Reports.ReportingFollowUp.Delete;

public sealed class DeleteReportingFollowUpCommand : ICommand<Result>
{
    public required Guid Id { get; set; } = Guid.Empty;
}