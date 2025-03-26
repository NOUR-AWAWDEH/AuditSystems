using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.Organisation.SubLocation.Create;

public sealed record class CreateSubLocationCommand : ICommand<Result<CreateSubLocationCommandResponse>>
{
    public required Guid LocationId { get; set; }
    public string Name { get; set; } = string.Empty;
}
