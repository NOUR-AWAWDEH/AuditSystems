using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.Organization.Location.Create;

public sealed record class CreateLocationCommand : ICommand<Result<CreateLocationCommandResponse>>
{
    public required string Name { get; set; } = string.Empty;
}
