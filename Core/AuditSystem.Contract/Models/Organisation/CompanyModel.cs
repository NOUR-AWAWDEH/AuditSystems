using AuditSystem.Contract.Models.Common;

namespace AuditSystem.Contract.Models.Organisation;

public class CompanyModel : BaseModel<Guid>
{
    public required string CompanyName { get; set; } = string.Empty;
}
