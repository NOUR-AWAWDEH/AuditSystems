using AuditSystem.Domain.Entities.Common;

namespace AuditSystem.Contract.Models.Common;

public sealed class RateModel : BaseModel<Guid>
{
    public RateLevel Level { get; set; }
}
