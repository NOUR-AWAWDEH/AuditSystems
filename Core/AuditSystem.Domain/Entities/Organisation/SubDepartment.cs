using AuditSystem.Domain.Entities.Common;

namespace AuditSystem.Domain.Entities.Organisation
{
    public class SubDepartment : Entity<Guid>
    {
        public string Name { get; set; } = string.Empty;
        public Guid DepartmentId { get; set; }
        
        public Department Department { get; set; } = null!;
    }
}