using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.Audit.AuditPlanSummary.Delete;

public sealed class DeleteAuditPlanSummaryCommand : ICommand<Result>
{
    public required Guid Id { get; set; } = Guid.Empty;
}
