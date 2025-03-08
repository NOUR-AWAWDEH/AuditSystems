using AuditSystem.Domain.Entities.Common;

namespace AuditSystem.Domain.Entities.Users
{
    public class UserManagement : Entity<Guid>
    {
        public Guid AdminSettingsID { get; set; }
        public Guid UserID { get; set; }
        public string Designation { get; set; } = string.Empty;
        
        public AdminSettings AdminSettings { get; set; } = null!;
        public User User { get; set; } = null!;
    }
}