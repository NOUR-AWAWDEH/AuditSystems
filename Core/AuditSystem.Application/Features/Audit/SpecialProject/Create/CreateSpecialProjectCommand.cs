using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.Audit.SpecialProject.Create;

public sealed record class CreateSpecialProjectCommand : ICommand<Result<CreateSpecialProjectCommandResponse>>
{
    public required Guid AuditUniverseId { get; set; }
    public string ProjectName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime StartDate { get; set; } = DateTime.UtcNow;
    public DateTime? EndDate { get; set; }
}