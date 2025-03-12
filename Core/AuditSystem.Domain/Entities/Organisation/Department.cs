using AuditSystem.Domain.Entities.Common;

namespace AuditSystem.Domain.Entities.Organisation
{
    public class Department : Entity<Guid>
    {
        public string Name { get; set; } = string.Empty;
        public Guid CompanyId { get; set; }
        
        public virtual Company Company { get; set; } = null!;
        public ICollection<User> Users { get; set; } = new List<User>();
    }
}