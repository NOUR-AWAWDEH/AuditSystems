using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.Processes.AuditProcess.Delete;

public sealed class DeleteAuditProcessCommand : ICommand<Result>
{
    public required Guid Id { get; set; } = Guid.Empty;
}
