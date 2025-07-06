using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.Audit.Objective.Delete;

public sealed class DeleteObjectiveCommand : ICommand<Result>
{
    public required Guid Id { get; set; } = Guid.Empty;
}