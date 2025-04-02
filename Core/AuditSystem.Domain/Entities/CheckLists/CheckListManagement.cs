using AuditSystem.Domain.Entities.Common;
using AuditSystem.Domain.Entities.Users;

namespace AuditSystem.Domain.Entities.Checklists;

public class ChecklistManagement : Entity<Guid>
{
    public required Guid AuditorSettingsId { get; set; }
    public required Guid ChecklistId { get; set; }

    public virtual AuditorSettings AuditorSettings { get; set; } = null!;
    public virtual Checklist Checklist { get; set; } = null!;
}