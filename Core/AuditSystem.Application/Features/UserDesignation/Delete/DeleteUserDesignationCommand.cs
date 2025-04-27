using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.UserDesignation.Delete;

public sealed class DeleteUserDesignationCommand : ICommand<Result>
{
    public required Guid Id { get; set; } = Guid.Empty;
}