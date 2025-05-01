using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.Reports.JobTimeAllocationReport.Delete;

public sealed class DeleteJobTimeAllocationReportCommand : ICommand<Result>
{
    public required Guid Id { get; set; } = Guid.Empty;
}
