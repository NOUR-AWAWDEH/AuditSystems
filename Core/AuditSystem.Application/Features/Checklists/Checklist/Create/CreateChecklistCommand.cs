using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.Checklists.Checklist.Create;

public sealed record class CreateChecklistCommand : ICommand<Result<CreateChecklistCommandResponse>>
{
    public required string Area { get; set; } = string.Empty;
    public required string Particulars { get; set; } = string.Empty;
    public required string Observation { get; set; } = string.Empty;
    public required Guid ChecklistCollectionId { get; set; }
}
