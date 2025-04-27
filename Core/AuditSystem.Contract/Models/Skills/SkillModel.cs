using AuditSystem.Contract.Models.Common;

namespace AuditSystem.Contract.Models.Skills;

public sealed class SkillModel : BaseModel<Guid>
{
    public required string Name { get; set; } = string.Empty;
    public required Guid CategoryId { get; set; }
    public required Guid SkillSetId { get; set; }
    public string Description { get; set; } = string.Empty;
}