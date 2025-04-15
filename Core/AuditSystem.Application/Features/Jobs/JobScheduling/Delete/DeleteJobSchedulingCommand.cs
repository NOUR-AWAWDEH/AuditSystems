using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.Jobs.JobScheduling.Delete;

public sealed class DeleteJobSchedulingCommand : ICommand<Result>
{
    public required Guid Id { get; set; } = Guid.Empty;
}
