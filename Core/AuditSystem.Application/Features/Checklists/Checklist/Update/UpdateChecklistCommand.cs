using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.Checklists.Checklist.Update;

public sealed record class UpdateChecklistCommand : ICommand<Result>
{
    public required Guid Id { get; init; } = Guid.Empty;
    public required string Area { get; set; } = string.Empty;
    public required string Particulars { get; set; } = string.Empty;
    public required string Observation { get; set; } = string.Empty;
    public required Guid ChecklistCollectionId { get; set; }
}
