using AuditSystem.Domain.Entities.Common;
namespace AuditSystem.Domain.Entities.Users
{
    public class AuditorSettings : Entity<Guid>
    {
        public Guid UserId { get; set; }

        public User User { get; set; } = null!;
    }
}