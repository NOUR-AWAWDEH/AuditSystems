using AuditSystem.Domain.Entities.Common;
using AuditSystem.Domain.Entities.Users;

namespace AuditSystem.Domain.Entities.Organization;

public class JobTitle : Entity<Guid>
{
    public string Name { get; set; } = string.Empty;

    public Guid CompanyId { get; set; }
    public Company Company { get; set; } = null!;

    public ICollection<AppUser> Users { get; set; } = [];
}