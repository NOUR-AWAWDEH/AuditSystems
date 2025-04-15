using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.Jobs.JobPrioritization.Delete;

public sealed class DeleteJobPrioritizationCommand : ICommand<Result>
{
    public required Guid Id { get; set; } = Guid.Empty;
}
