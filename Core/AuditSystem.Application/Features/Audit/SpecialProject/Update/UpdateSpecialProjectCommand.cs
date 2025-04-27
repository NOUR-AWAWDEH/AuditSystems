using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.Audit.SpecialProject.Update;

public sealed record class UpdateSpecialProjectCommand : ICommand<Result>
{
    public required Guid Id { get; init; } = Guid.Empty;
    public required Guid AuditUniverseId { get; set; }
    public required string ProjectName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public required DateTime StartDate { get; set; } = DateTime.UtcNow;
    public required DateTime? EndDate { get; set; }
}
