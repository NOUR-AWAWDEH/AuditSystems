namespace AuditSystem.Contract.Models.Common;

public sealed class RateModel : BaseModel<Guid>
{
    public required string Level { get; set; } = string.Empty;
}
