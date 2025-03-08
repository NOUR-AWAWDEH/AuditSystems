using AuditSystem.Domain.Entities.Common;
using AuditSystem.Domain.Entities.Users;

namespace AuditSystem.Domain.Entities.Organisation
{
    public class Location : Entity<Guid>
    {
        public Guid AdminSettingsID { get; set; }
        public string Name { get; set; } = string.Empty;
        
        public AdminSettings AdminSettings { get; set; } = null!;
    }
}