using AuditSystem.Contract.Models.Common;

namespace AuditSystem.Contract.Models.Checklists;

public sealed class RemarkModel : BaseModel<Guid>
{
    public required Guid CheckListManagementId { get; set; }
    public string RemarkCommants { get; set; } = string.Empty;
}
