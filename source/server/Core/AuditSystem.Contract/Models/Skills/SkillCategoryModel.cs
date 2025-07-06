using AuditSystem.Contract.Models.Common;

namespace AuditSystem.Contract.Models.Skills;

public sealed class SkillCategoryModel : BaseModel<Guid>
{
    public required string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}
