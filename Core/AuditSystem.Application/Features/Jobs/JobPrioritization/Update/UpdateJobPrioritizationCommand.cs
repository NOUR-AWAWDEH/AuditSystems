using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.Jobs.JobPrioritization.Update;

public sealed record class UpdateJobPrioritizationCommand : ICommand<Result>
{
    public required Guid Id { get; init; } = Guid.Empty;
    public required string AuditableUnit { get; set; } = string.Empty;
    public required bool SelectForAudit { get; set; }
    public string Comments { get; set; } = string.Empty;
    public required DateOnly SelectedYear { get; set; }
    public required Guid BusinessObjectiveId { get; set; }
    public required Guid RatingId { get; set; }
}
