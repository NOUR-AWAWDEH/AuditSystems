using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.Organization.SubLocation.Update;

public sealed record class UpdateSubLocationCommand : ICommand<Result>
{
    public required Guid Id { get; init; } = Guid.Empty;
    public required string Name { get; set; } = string.Empty;
    public required Guid LocationId { get; set; }
}