using AuditSystem.Contract.Models.Common;

namespace AuditSystem.Contract.Models.Skills;

public sealed class SkillModel : BaseModel<Guid>
{
    public string Name { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}
