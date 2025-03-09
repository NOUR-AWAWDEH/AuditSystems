using AuditSystem.Domain.Entities.Common;
using AuditSystem.Domain.Entities.Users;

namespace AuditSystem.Domain.Entities.CheckLists
{
    public class CheckListManagement : Entity<Guid>
    {
        public Guid AuditorSettingsId { get; set; } // Audit
        public Guid ChecklistId { get; set; }

        public AuditorSettings AuditorSettings { get; set; } = null!;
        public Checklist Checklist { get; set; } = null!;
    }
}