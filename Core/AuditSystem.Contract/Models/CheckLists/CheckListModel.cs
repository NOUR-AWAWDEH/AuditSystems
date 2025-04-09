using AuditSystem.Contract.Models.Common;

namespace AuditSystem.Contract.Models.Checklists;

public sealed class ChecklistModel : BaseModel<Guid>
{
    public required string Area { get; set; } = string.Empty;
    public required string Particulars { get; set; } = string.Empty;
    public required string Observation { get; set; } = string.Empty;
    public required Guid ChecklistCollectionId { get; set; }

    public required Guid ChecklistManagementId { get; set; }
}
