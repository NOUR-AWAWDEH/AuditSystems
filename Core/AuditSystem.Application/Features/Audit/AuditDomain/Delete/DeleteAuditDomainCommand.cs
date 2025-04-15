using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.Audit.AuditDomain.Delete;

public sealed class DeleteAuditDomainCommand : ICommand<Result>
{
    public required Guid Id { get; set; } = Guid.Empty;
}
