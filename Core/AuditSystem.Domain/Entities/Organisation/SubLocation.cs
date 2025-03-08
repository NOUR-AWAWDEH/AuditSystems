using AuditSystem.Domain.Entities.Common;
using AuditSystem.Domain.Entities.Users;

namespace AuditSystem.Domain.Entities.Organisation
{
    public class SubLocation : Entity<Guid>
    {
        public Guid AdminSettingsID { get; set; }
        public Guid LocationId { get; set; }
        public string Name { get; set; } = string.Empty;
        
        public AdminSettings AdminSettings { get; set; } = null!;
        public Location Location { get; set; } = null!;
    }
}