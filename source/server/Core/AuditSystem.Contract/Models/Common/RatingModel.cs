namespace AuditSystem.Contract.Models.Common;

public sealed class RatingModel : BaseModel<Guid>
{
    public string Level { get; set; } = string.Empty;
}
