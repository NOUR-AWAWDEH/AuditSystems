using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.Organization.Location.Create;

public sealed record class CreateLocationCommand : ICommand<Result<CreateLocationCommandResponse>>
{
    public required string Name { get; set; } = string.Empty;
    public string? Address { get; set; }
    
    // Foreign key to Company
    public required Guid CompanyId { get; set; }
    
    // Foreign key to User (who created/owns this location)
    public required Guid UserId { get; set; }
}