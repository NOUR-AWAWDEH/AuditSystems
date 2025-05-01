using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.Jobs.AuditJob.Delete;

public sealed class DeleteAuditJobCommand : ICommand<Result>
{
    public required Guid Id { get; set; } = Guid.Empty;
}
