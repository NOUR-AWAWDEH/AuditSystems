using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.Reports.PlanningReport.Delete;

public sealed class DeletePlanningReportCommand : ICommand<Result>
{
    public required Guid Id { get; set; } = Guid.Empty;
}