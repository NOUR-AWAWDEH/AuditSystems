using AuditSystem.Contract.Models.Common;

namespace AuditSystem.Contract.Models.Organisation;

public sealed class DepartmentModel : BaseModel<Guid>
{
    public required string Name { get; set; } = string.Empty;
}
