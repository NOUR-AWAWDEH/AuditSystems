using AuditSystem.Contract.Models.Common;

namespace AuditSystem.Contract.Models.Organisation;

public sealed class SubDepartmentModel : BaseModel<Guid>
{
    public required string Name { get; set; } = string.Empty;
    public required Guid DepartmentId { get; set; }
}
