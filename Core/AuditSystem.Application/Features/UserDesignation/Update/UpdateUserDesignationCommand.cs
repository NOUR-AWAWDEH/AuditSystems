using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.UserDesignation.Update;

public sealed record class UpdateUserDesignationCommand : ICommand<Result>
{
    public required Guid Id { get; init; } = Guid.Empty;
    public required string Designation { get; set; } = string.Empty;
    public required string Level { get; set; }
    public required bool IsActive { get; set; } = true;
    public required Guid UserId { get; set; }
    public string Description { get; set; } = string.Empty;
}