using AuditSystem.Contract.Models.Common;

namespace AuditSystem.Contract.Models.CheckLists;

public sealed class RemarkModel : BaseModel<Guid>
{
    public required Guid CheckListManagementId { get; set; }
    public required string Remarkcommants { get; set; } = string.Empty;
}
