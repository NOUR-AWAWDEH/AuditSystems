using AuditSystem.Domain.Entities.Common;
using AuditSystem.Domain.Entities.Users;

namespace AuditSystem.Domain.Entities.Organisation;

public class Department : Entity<Guid>
{
    public string Name { get; set; } = string.Empty;
    public required Guid CompanyId { get; set; }
    
    public virtual Company Company { get; set; } = null!;
    public ICollection<User> Users { get; set; } = new List<User>();
}