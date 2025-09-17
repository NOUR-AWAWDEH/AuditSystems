using AuditSystem.Domain.Entities.Common;

namespace AuditSystem.Contract.Models.Common;

public sealed class RateModel : BaseModel<Guid>
{
    public required RateLevel Level { get; set; }
}
