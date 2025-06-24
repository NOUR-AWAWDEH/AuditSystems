using AuditSystem.Domain.Entities.Common;

namespace AuditSystem.Domain.Entities.Organization;

public class Location : Entity<Guid>
{
    public required string Name { get; set; } = string.Empty;
    public string? Address { get; set; }
    
    // Foreign key to Company
    public required Guid CompanyId { get; set; } // Many-to-One
    
    // Foreign key to User (who created/owns this location)
    public required Guid UserId { get; set; }
    
    // Navigation properties
    public virtual Company Company { get; set; } = null!;
    public virtual User User { get; set; } = null!;
}