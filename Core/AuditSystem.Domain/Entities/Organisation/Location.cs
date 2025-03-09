using AuditSystem.Domain.Entities.Common;
using AuditSystem.Domain.Entities.Users;

namespace AuditSystem.Domain.Entities.Organisation
{
    public class Location : Entity<Guid>
    {
        public Guid AuditorSettingsID { get; set; }
        public string Name { get; set; } = string.Empty;
        
        public AuditorSettings AuditorSettings { get; set; } = null!;
    }
}