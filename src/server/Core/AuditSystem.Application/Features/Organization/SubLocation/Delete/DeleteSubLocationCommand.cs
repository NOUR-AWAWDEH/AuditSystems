using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.Organization.SubLocation.Delete;

public sealed class DeleteSubLocationCommand : ICommand<Result>
{
    public required Guid Id { get; set; } = Guid.Empty;
}
