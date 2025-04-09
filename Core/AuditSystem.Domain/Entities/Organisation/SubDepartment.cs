using AuditSystem.Domain.Entities.Common;
using AuditSystem.Domain.Entities.Users;

namespace AuditSystem.Domain.Entities.Organisation;

public class SubDepartment : Entity<Guid>
{
    public required string Name { get; set; } = string.Empty;
    public required Guid DepartmentId { get; set; }
    
    public virtual Department Department { get; set; } = null!;

}