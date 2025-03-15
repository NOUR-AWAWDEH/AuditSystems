using AuditSystem.Contract.Models.Common;

namespace AuditSystem.Contract.Models.Organisation;

public class DepartmentModel : BaseModel<Guid>
{
    public required string Name { get; set; } = string.Empty;
    public required Guid CompanyId { get; set; }
}
