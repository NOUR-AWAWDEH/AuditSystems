using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.Organisation.LocationModle.Create;

public sealed record class CreateLocationCommand : ICommand<Result<CreateLocationCommandResponse>>
{
    public required string Name { get; set; } = string.Empty;
}
