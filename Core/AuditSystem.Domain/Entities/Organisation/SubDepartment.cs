using AuditSystem.Domain.Entities.Common;
using AuditSystem.Domain.Entities.Users;

namespace AuditSystem.Domain.Entities.Organisation
{
    public class SubDepartment : Entity<Guid>
    {
        public string Name { get; set; } = string.Empty;
        public Guid DepartmentId { get; set; }
        
        public virtual Department Department { get; set; } = null!;
        public ICollection<User> Users { get; set; } = new List<User>();
    }
}