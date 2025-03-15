using AuditSystem.Contract.Models.Common;

namespace AuditSystem.Contract.Models.CheckLists;

public sealed class CheckListModel : BaseModel<Guid>
{
    public required string Area { get; set; } = string.Empty;
    public string Particulars { get; set; } = string.Empty;
    public string Observation { get; set; } = string.Empty;
}
