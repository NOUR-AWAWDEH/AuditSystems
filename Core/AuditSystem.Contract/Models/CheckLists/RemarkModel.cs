using AuditSystem.Contract.Models.Common;

namespace AuditSystem.Contract.Models.Checklists;

public sealed class RemarkModel : BaseModel<Guid>
{
    public string RemarkCommants { get; set; } = string.Empty;
    public required Guid CheckListCollectionId { get; set; }
}
