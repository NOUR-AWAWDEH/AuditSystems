using AuditSystem.Contract.Models.Common;

namespace AuditSystem.Contract.Models.Checklists;

public sealed class ChecklistModel : BaseModel<Guid>
{
    public required string Area { get; set; } = string.Empty;
    public string Particulars { get; set; } = string.Empty;
    public string Observation { get; set; } = string.Empty;
}
