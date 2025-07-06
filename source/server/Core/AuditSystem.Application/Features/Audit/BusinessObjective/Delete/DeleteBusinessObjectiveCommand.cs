using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.Audit.BusinessObjective.Delete;

public sealed class DeleteBusinessObjectiveCommand : ICommand<Result>
{
    public required Guid Id { get; set; } = Guid.Empty;
}