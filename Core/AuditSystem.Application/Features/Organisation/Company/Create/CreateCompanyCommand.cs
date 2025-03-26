using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.Organisation.Company.Create;

public sealed record class CreateCompanyCommand : ICommand<Result<CreateCompanyCommandResponse>>
{
    public string Name { get; set; } = string.Empty;
}