using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.Organisation.Location.Create;

public sealed record class CreateLocationCommand : ICommand<Result<CreateLocationCommandResponse>>
{
    public required Guid AuditorSettingsId { get; set; }
    public string Name { get; set; } = string.Empty;
}
