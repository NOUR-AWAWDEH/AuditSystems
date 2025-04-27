using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.Organization.Location.Delete;

public sealed class DeleteLocationCommand : ICommand<Result>
{
    public required Guid Id { get; set; } = Guid.Empty;
}
