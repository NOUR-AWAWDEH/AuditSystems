using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.Organization.SubLocation.Delete;

public sealed record class DeleteSubLocationCommand : ICommand<Result>
{
    public required Guid Id { get; set; } = Guid.Empty;
    public Guid UserId { get; set; } = Guid.Empty;
}
