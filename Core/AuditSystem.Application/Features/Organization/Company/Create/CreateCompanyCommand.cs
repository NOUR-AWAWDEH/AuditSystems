using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.Organization.Company.Create;

public sealed record class CreateCompanyCommand : ICommand<Result<CreateCompanyCommandResponse>>
{
    public required string Name { get; set; } = string.Empty;
}