using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.Checklists.Checklist.Delete;

public sealed class DeleteChecklistCommand : ICommand<Result>
{
    public required Guid Id { get; set; } = Guid.Empty;
}
