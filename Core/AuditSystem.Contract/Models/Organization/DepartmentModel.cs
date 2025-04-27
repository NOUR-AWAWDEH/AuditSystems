using AuditSystem.Contract.Models.Common;

namespace AuditSystem.Contract.Models.Organization;

public sealed class DepartmentModel : BaseModel<Guid>
{
    public required string Name { get; set; } = string.Empty;
}
