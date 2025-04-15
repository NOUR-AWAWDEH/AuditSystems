using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.Audit.SpecialProject.Delete;

public sealed class DeleteSpecialProjectCommand : ICommand<Result>
{
    public required Guid Id { get; set; } = Guid.Empty;
}
