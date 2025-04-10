using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.Audit.SpecialProject.Create;

public sealed record class CreateSpecialProjectCommand : ICommand<Result<CreateSpecialProjectCommandResponse>>
{
    public required Guid AuditUniverseId { get; set; }
    public required string ProjectName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public required DateTime StartDate { get; set; } = DateTime.UtcNow;
    public required DateTime? EndDate { get; set; }
}