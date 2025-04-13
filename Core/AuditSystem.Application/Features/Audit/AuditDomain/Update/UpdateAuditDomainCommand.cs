using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.Audit.AuditDomain.Update;

public sealed record class UpdateAuditDomainCommand : ICommand<Result>
{
    public required Guid Id { get; init; } = Guid.Empty;
    public required string Name { get; init; } = string.Empty;
}