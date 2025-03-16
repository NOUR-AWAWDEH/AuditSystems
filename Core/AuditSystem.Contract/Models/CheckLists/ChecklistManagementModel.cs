using AuditSystem.Contract.Models.Common;

namespace AuditSystem.Contract.Models.Checklists;

public sealed class ChecklistManagementModel : BaseModel<Guid>
{
    public required Guid AuditorSettingsId { get; init; }
    public required Guid ChecklistId { get; init; }
}
