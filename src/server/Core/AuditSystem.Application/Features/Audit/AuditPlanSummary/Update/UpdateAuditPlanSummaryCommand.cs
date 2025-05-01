using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.Audit.AuditPlanSummary.Update;

public sealed record class UpdateAuditPlanSummaryCommand : ICommand<Result>
{
    public required Guid Id { get; init; } = Guid.Empty;
    public required string Component { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string ExampleDetails { get; set; } = string.Empty;
}