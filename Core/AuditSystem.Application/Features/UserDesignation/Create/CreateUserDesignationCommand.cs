using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.UserDesignation.Create;

public sealed record class CreateUserDesignationCommand : ICommand<Result<CreateUserDesignationCommandResponse>>
{
    public required string Designation { get; set; } = string.Empty;
    public required string Level { get; set; }
    public required bool IsActive { get; set; } = true;
    public required Guid UserId { get; set; }
    public string Description { get; set; } = string.Empty;
}