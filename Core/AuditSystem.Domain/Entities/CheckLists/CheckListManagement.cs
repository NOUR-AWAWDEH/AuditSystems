using AuditSystem.Domain.Entities.Common;
using AuditSystem.Domain.Entities.Users;

namespace AuditSystem.Domain.Entities.CheckLists
{
    public class CheckListManagement : Entity<Guid>
    {
        public Guid AdminSettingsId { get; set; }
        public Guid ChecklistId { get; set; }

        public AdminSettings AdminSettings { get; set; } = null!;
        public Checklist Checklist { get; set; } = null!;
    }
}