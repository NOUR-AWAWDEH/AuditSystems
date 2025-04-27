using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.Organization.Location.Update;

public sealed record class UpdateLocationCommand : ICommand<Result>
{
    public required Guid Id { get; init; } = Guid.Empty;
    public required string Name { get; set; } = string.Empty;
}
