using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.Organization.Location.Update;

public sealed record class UpdateLocationCommand : ICommand<Result>
{
    public required Guid Id { get; init; } = Guid.Empty;
    public required string Name { get; init; } = string.Empty;
    public string? Address { get; init; }
    public required Guid CompanyId { get; init; } = Guid.Empty;
    public required Guid UserId { get; init; } = Guid.Empty;
}
