using AuditSystem.Domain.Entities.Common;
using AuditSystem.Domain.Entities.Users;

namespace AuditSystem.Domain.Entities.Organization;

public class Location : Entity<Guid>
{
    public required string Name { get; set; } = string.Empty;
    public string? Address { get; set; }
    public required Guid CompanyId { get; set; }
    public required Guid UserId { get; set; }
    
    // Navigation properties
    public virtual Company Company { get; set; } = null!;
    public virtual User User { get; set; } = null!;
}