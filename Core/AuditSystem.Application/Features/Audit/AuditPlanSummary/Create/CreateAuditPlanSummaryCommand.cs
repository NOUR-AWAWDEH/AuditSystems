using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.Audit.AuditPlanSummary.Create;

public sealed record class CreateAuditPlanSummaryCommand : ICommand<Result<CreateAuditPlanSummaryCommandResponse>>
{
    public required Guid AuditorSettingsId { get; set; }
    public string Component { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string ExampleDetails { get; set; } = string.Empty;
}
