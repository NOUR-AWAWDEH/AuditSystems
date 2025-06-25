using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.Organization.Location.Create;

public sealed record class CreateLocationCommand : ICommand<Result<CreateLocationCommandResponse>>
{
    public required string Name { get; set; } = string.Empty;
    public string? Address { get; set; } 
    public required Guid CompanyId { get; set; } 
    public required Guid UserId { get; set; }
}