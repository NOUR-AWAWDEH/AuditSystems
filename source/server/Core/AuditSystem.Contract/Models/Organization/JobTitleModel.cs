using AuditSystem.Contract.Models.Common;

namespace AuditSystem.Contract.Models.Organization;

public sealed class JobTitleModel : BaseModel<Guid>
{
    public required string Name { get; set; }
    public required Guid CompanyId { get; set; }
}
