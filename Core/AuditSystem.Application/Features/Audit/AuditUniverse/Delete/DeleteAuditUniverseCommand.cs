using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.Audit.AuditUniverse.Delete;

public sealed class DeleteAuditUniverseCommand : ICommand<Result>
{
    public required Guid Id { get; set; } = Guid.Empty;
}
