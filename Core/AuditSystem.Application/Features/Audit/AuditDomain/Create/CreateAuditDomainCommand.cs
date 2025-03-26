using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.Audit.AuditDomain.Create;

public sealed record class CreateAuditDomainCommand : ICommand<Result<CreateAuditDomainCommandResponse>>
{
    public required string DomainName { get; set; } = string.Empty;
}